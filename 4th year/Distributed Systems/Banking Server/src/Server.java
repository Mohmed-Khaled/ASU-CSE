import java.net.ServerSocket;
import java.net.Socket;
import java.util.Scanner;

public class Server {
    public static void main(String[] args) {
        try {
            //1.Create Server Socket
            Scanner cin = new Scanner(System.in);
            System.out.print("Please enter database name: ");
            String db = cin.nextLine();
            BankDatabase bdb = new BankDatabase(db);
            System.out.print("Please enter port number: ");
            String port = cin.nextLine();
            ServerSocket server = new ServerSocket(Integer.valueOf(port));
            System.out.println("Server Ready");
            while (true) {
                //2.accept connection
                Socket client = server.accept();
                System.out.println("Client Arrived");
                ClientHandler ch = new ClientHandler(client, bdb);
                ch.start();
            }
        } catch (Exception e) {
            System.out.println("Something Went Wrong: " + e.toString());
        }
    }
}
