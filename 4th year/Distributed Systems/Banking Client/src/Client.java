import java.io.Console;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.Socket;
import java.util.Scanner;

public class Client {
    public static void main(String[] args) {
        try {
            Scanner cin = new Scanner(System.in);
            //1.create client socket and connect to server
            System.out.println("Please enter IP:port you want to connect to: ");
            String address = cin.nextLine();
            String[] inputs = address.split(":");
            Socket client = new Socket(inputs[0], Integer.valueOf(inputs[1]));
            //2.create comm streams
            DataInputStream dis = new DataInputStream(client.getInputStream());
            DataOutputStream dos = new DataOutputStream(client.getOutputStream());
            //inform server that this is a client connection
            dos.writeUTF("CLIENT-CONN");
            //3.perform I/O with server
            while (true) {
                //receive msg from server
                String serverCommand =  dis.readUTF();
                System.out.println(serverCommand);
                if(serverCommand.contains("Please")) {
                    String userInput;
                    if(serverCommand.contains("enter password"))
                    {
                        Console console = System.console();
                        if(console != null)
                        {
                            char[] passString = console.readPassword();
                            userInput = new String(passString);
                        }
                        else
                        {
                            userInput = cin.nextLine();
                        }
                    }
                    else
                    {
                        userInput = cin.nextLine();
                    }
                    dos.writeUTF(userInput);
                }
                else if(serverCommand.equalsIgnoreCase("Wrong password for 3 times, try again later"))
                {
                    break;
                }
                else if(serverCommand.equalsIgnoreCase("disconnected"))
                {
                    break;
                }
            }
            //4.terminate connection with server
            client.close();
            dis.close();
            dos.close();
        } catch (Exception e) {
            System.out.println("Something went wrong: " + e.toString());
        }
    }
}
