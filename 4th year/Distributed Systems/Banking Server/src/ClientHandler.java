import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.Socket;
import java.sql.Timestamp;

class ClientHandler extends Thread {

    private Socket client;
    private BankDatabase bdb;

    ClientHandler(Socket c, BankDatabase bdb) {
        this.client = c;
        this.bdb = bdb;
    }

    @Override
    public void run() {
        try {
            //3.create comm channel
            DataInputStream dis = new DataInputStream(client.getInputStream());
            DataOutputStream dos = new DataOutputStream(client.getOutputStream());
            String type = dis.readUTF();
            while (true) {
                //4.perform I/O
                if(type.equals("CLIENT-CONN")) {
                    //a.asks for account no.
                    dos.writeUTF("Please enter username: ");
                    String username = dis.readUTF();
                    if(bdb.checkUser(username)) {
                        dos.writeUTF("Valid Username: " + username + "."
                                + "\nPlease enter password: ");
                    } else {
                        do {
                            dos.writeUTF("Invalid Username: " + username + "."
                                    + "\nPlease enter username again: ");
                            username = dis.readUTF();
                        } while(!bdb.checkUser(username));
                        dos.writeUTF("Valid Username: " + username
                                + "\nPlease enter password: ");
                    }
                    String password = dis.readUTF();
                    if(bdb.checkUser(username,password)) {
                        dos.writeUTF("Connected.\nWelcome " + username + "."
                                + "\nPlease choose operation: "
                                + "\n1. Check Balance"
                                + "\n2. View Transactions History"
                                + "\n3. Deposit <amount>"
                                + "\n4. Withdraw <amount>"
                                + "\n5. Internal Transfer <target username>,<amount>"
                                + "\n5. External Transfer <target bank IP:port>,<target bank>,<target username>,<amount>"
                                + "\n7. Quit");
                    } else {
                        int trials = 3;
                        do {
                            trials--;
                            if(trials == 0) {
                                dos.writeUTF("Wrong password for 3 times, try again later");
                                client.close();
                                dis.close();
                                dos.close();
                                return;
                            }
                            dos.writeUTF("Invalid password for " + username + "."
                                    + "\nPlease enter password again: ");
                            password = dis.readUTF();
                        } while(!bdb.checkUser(username,password));
                        dos.writeUTF("Connected.\nWelcome " + username + "."
                                + "\nPlease choose operation: "
                                + "\n1. Check Balance"
                                + "\n2. View Transactions History"
                                + "\n3. Deposit <amount>"
                                + "\n4. Withdraw <amount>"
                                + "\n5. Internal Transfer <target username>,<amount>"
                                + "\n5. External Transfer <target bank IP:port>,<target bank>,<target username>,<amount>"
                                + "\n7. Quit");
                    }
                    while(true){
                        String operation = dis.readUTF();
                        if (operation.equalsIgnoreCase("Quit")) {
                            dos.writeUTF("disconnected");
                            client.close();
                            dis.close();
                            dos.close();
                            return;
                        } else if (operation.equalsIgnoreCase("Check balance")) {
                            String balance = bdb.getBalance(username);
                            if(balance.contains("later")) {
                                dos.writeUTF(balance + "\n");
                            } else {
                                dos.writeUTF("Your Balance: " + balance + ".\n");
                            }
                        } else if (operation.equalsIgnoreCase("View Transactions History")) {
                            dos.writeUTF("Your Transactions History:\n");
                            dos.writeUTF("\tDeposit:");
                            dos.writeUTF(bdb.getDepositHistory(username));
                            dos.writeUTF("\tWithdraw:");
                            dos.writeUTF(bdb.getWithdrawHistory(username));
                            dos.writeUTF("\tIncoming Internal Transfer:");
                            dos.writeUTF(bdb.getIncomingInternalTransferHistory(username));
                            dos.writeUTF("\tOutgoing Internal Transfer:");
                            dos.writeUTF(bdb.getOutgoingInternalTransferHistory(username));
                            dos.writeUTF("\tIncoming External Transfer:");
                            dos.writeUTF(bdb.getIncomingExternalTransferHistory(username));
                            dos.writeUTF("\tOutgoing External Transfer:");
                            dos.writeUTF(bdb.getOutgoingExternalTransferHistory(username));
                        } else if (operation.contains("Deposit")) {
                            String[] arg = operation.split(" ");
                            if(bdb.deposit(username,arg[1])){
                                dos.writeUTF("Deposit of " + arg[1] + " successful.\n"+
                                        "Current Balance: " + bdb.getBalance(username) + ".\n");
                            } else {
                                dos.writeUTF("Deposit of " + arg[1] + " failed, try again later.\n");
                            }
                        } else if (operation.contains("Withdraw")) {
                            String[] arg = operation.split(" ");
                            int balance = Integer.valueOf(bdb.getBalance(username));
                            if(balance < Integer.valueOf(arg[1])) {
                                dos.writeUTF("Your balance is less than the amount you want to withdraw.\n" +
                                        "Current Balance: " + balance + ".\n");
                            } else if(bdb.withdraw(username,arg[1])){
                                dos.writeUTF("Withdraw of " + arg[1] + " successful.\n" +
                                        "Current Balance: " + bdb.getBalance(username) + ".\n");
                            } else {
                                dos.writeUTF("Withdraw of " + arg[1] + " failed, try again later.\n");
                            }
                        } else if (operation.contains("Internal Transfer")) {
                            String[] tmpArg = operation.split(" ");
                            String[] arg = tmpArg[2].split(",");
                            int balance = Integer.valueOf(bdb.getBalance(username));
                            if(balance < Integer.valueOf(arg[1])) {
                                dos.writeUTF("Your balance is less than the amount you want to transfer.\n" +
                                        "Current Balance: " + balance + ".\n");
                            } else if(bdb.InternalTransfer(username,arg[0],arg[1])){
                                dos.writeUTF("Transfer of " + arg[1] + " to " + arg[0] + " successful.\n");
                            } else {
                                dos.writeUTF(
                                        "Transfer of " + arg[1] + " to " + arg[0] + " failed, try again later\n");
                            }
                        } else if (operation.contains("External Transfer")) {
                            String[] tmpArg = operation.split(" ");
                            String[] arg = tmpArg[2].split(",");
                            String[] inputs = arg[0].split(":");
                            String otherBank = arg[1];
                            String otherUser = arg[2];
                            String amount = arg[3];
                            int balance = Integer.valueOf(bdb.getBalance(username));
                            if(balance < Integer.valueOf(amount)) {
                                dos.writeUTF("Your balance is less than the amount you want to transfer.\n" +
                                        "Current Balance: " + balance + ".\n");
                            } else {
                                try {
                                    Socket p2p = new Socket(inputs[0], Integer.valueOf(inputs[1]));
                                    DataInputStream p2pDis = new DataInputStream(p2p.getInputStream());
                                    DataOutputStream p2pDos = new DataOutputStream(p2p.getOutputStream());
                                    p2pDos.writeUTF("BANK-CONN");
                                    // p2p protocol start
                                    String auth = bdb.getBankAuth(otherBank);
                                    p2pDos.writeUTF(bdb.getDbName() + ":" + auth);
                                    String response1 = p2pDis.readUTF();
                                    if(response1.equalsIgnoreCase("Wrong Auth")){
                                        dos.writeUTF("Transfer of " + amount + " to " + otherUser +
                                                " in "+ otherBank + " failed, try again later\n");
                                    } else if(response1.equalsIgnoreCase("Enter Transaction")) {
                                        p2pDos.writeUTF(otherUser + ":" + username + ":" + amount);
                                        String response2 = p2pDis.readUTF();
                                        if(response2.equalsIgnoreCase("Failed")){
                                            dos.writeUTF("Transfer of " + amount + " to " + otherUser +
                                                    " in "+ otherBank + " failed, try again later\n");
                                        } else if(response2.equalsIgnoreCase("Done")) {
                                            if(bdb.OutgoingExternalTransfer(username,otherBank,otherUser,amount)) {
                                                p2pDos.writeUTF("Done");
                                                dos.writeUTF("Transfer of " + amount + " to " + otherUser +
                                                        " in "+ otherBank + " successful.\n");
                                            } else {
                                                p2pDos.writeUTF("Failed");
                                                dos.writeUTF("Transfer of " + amount + " to " + otherUser +
                                                        " in "+ otherBank + " failed, try again later\n");
                                            }
                                        }
                                    }
                                    // p2p protocol end
                                    p2p.close();
                                    p2pDis.close();
                                    p2pDos.close();
                                } catch (Exception e){
                                    if(e.toString().contains("Connection refused: connect")) {
                                        dos.writeUTF("Could not connect to  " + otherBank + ". Try again later.\n");
                                    }
                                }
                            }
                        }
                        dos.writeUTF("Please choose operation: "
                                + "\n1. Check Balance"
                                + "\n2. View Transactions History"
                                + "\n3. Deposit <amount>"
                                + "\n4. Withdraw <amount>"
                                + "\n5. Internal Transfer <target username>,<amount>"
                                + "\n5. External Transfer <target bank IP:port>,<target bank>,<target username>,<amount>"
                                + "\n7. Quit");
                    }
                } else if(type.equals("BANK-CONN")) {
                    System.out.println("Another bank connected.");
                    String tmpBankAuth = dis.readUTF();
                    String[] bankAuth = tmpBankAuth.split(":");
                    if(bdb.bankAuthenticate(bankAuth[0],bankAuth[1])){
                        dos.writeUTF("Enter Transaction");
                        String tmpTransaction = dis.readUTF();
                        String[] transaction = tmpTransaction.split(":");
                        Timestamp time =
                                bdb.IncomingExternalTransfer(transaction[0],bankAuth[0],transaction[1],transaction[2]);
                        if(!time.equals(new Timestamp(0))){
                            dos.writeUTF("Done");
                            String response = dis.readUTF();
                            if(response.equalsIgnoreCase("Done")){
                                break;
                            } else if (response.equalsIgnoreCase("Done")) {
                                bdb.removeIncomingExternalTransaction(transaction[0],time,transaction[2]);
                                break;
                            }
                        } else {
                            dos.writeUTF("Failed");
                            break;
                        }
                    } else {
                        dos.writeUTF("Wrong Auth");
                        break;
                    }
                }
            }
            //5.terminate connection with client
            client.close();
            dis.close();
            dos.close();
        }
        catch (Exception e) {
            System.out.println("Something Went Wrong: " + e.toString());
            e.printStackTrace();
        }
    }
}