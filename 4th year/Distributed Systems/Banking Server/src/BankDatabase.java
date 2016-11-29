import java.sql.*;
import java.util.Date;

class BankDatabase {
    private static final String BASE_URL = "jdbc:mysql://localhost/";
    private static final String USER_NAME = "root";
    private static final String PASSWORD = "";
    private String dbName;
    private Connection conn;

    BankDatabase(String dbName){
        this.dbName = dbName;
        try {
            conn = DriverManager.getConnection(BASE_URL + this.dbName,USER_NAME,PASSWORD);
            conn.setAutoCommit(false);
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    String getDbName() {
        return dbName;
    }

    boolean checkUser(String username) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT user_name FROM accounts WHERE user_name = ?")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                if(rs.first())
                    return true;
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return false;
    }

    boolean checkUser(String username, String password) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT user_name FROM accounts WHERE user_name = ? and password = ?")) {
            preparedStatement.setString(1,username);
            preparedStatement.setString(2,password);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                if(rs.first())
                    return true;
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return false;
    }

    String getBalance(String username) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT user_name,balance FROM accounts WHERE user_name = ?")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                if( rs.first()) {
                    return rs.getString("balance");
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return "Could not get balance, try again later.";
    }

    String getDepositHistory(String username) {
        String result = "\n\t\tNo transactions.";
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT transaction_time,amount FROM transactions WHERE user_name = ? and type = 'DP'")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                String tmpResult = "";
                while( rs.next()) {
                     tmpResult += "\n\t\t" + "Amount: " + rs.getString("amount")
                             + " Time: " + rs.getString("transaction_time");
                    result = tmpResult;
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }

    String getWithdrawHistory(String username) {
        String result = "\n\t\tNo transactions.";
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT transaction_time,amount FROM transactions WHERE user_name = ? and type = 'WD'")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                String tmpResult = "";
                while( rs.next()) {
                    tmpResult += "\n\t\t" + "Amount: " + rs.getString("amount")
                            + " Time: " + rs.getString("transaction_time");
                    result = tmpResult;
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }

    String getIncomingInternalTransferHistory(String username) {
        String result = "\n\t\tNo transactions.";
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT internaltransactions.user_name,transactions.transaction_time," +
                                     "transactions.amount FROM internaltransactions JOIN transactions " +
                                     "ON transactions.user_name = internaltransactions.user_name " +
                                     "and transactions.transaction_time = internaltransactions.transaction_time " +
                                     "WHERE internaltransactions.target_user = ? and transactions.type = 'IT'")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                String tmpResult = "";
                while( rs.next()) {
                    tmpResult += "\n\t\t" + "Amount: " + rs.getString("amount")
                            + " From: " + rs.getString("user_name")
                            + " Time: " + rs.getString("transaction_time");
                    result = tmpResult;
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }

    String getOutgoingInternalTransferHistory(String username) {
        String result = "\n\t\tNo transactions.";
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT internaltransactions.target_user,transactions.transaction_time," +
                                     "transactions.amount FROM internaltransactions JOIN transactions " +
                                     "ON transactions.user_name = internaltransactions.user_name " +
                                     "and transactions.transaction_time = internaltransactions.transaction_time " +
                                     "WHERE internaltransactions.user_name = ? and transactions.type = 'IT'")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                String tmpResult = "";
                while( rs.next()) {
                    tmpResult += "\n\t\t" + "Amount: " + rs.getString("amount")
                            + " To: " + rs.getString("target_user")
                            + " Time: " + rs.getString("transaction_time");
                    result = tmpResult;
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }

    String getIncomingExternalTransferHistory(String username) {
        String result = "\n\t\tNo transactions.";
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT externaltransactions.target_user,externaltransactions.target_bank," +
                                     "transactions.transaction_time,transactions.amount " +
                                     "FROM transactions JOIN externaltransactions " +
                                     "ON transactions.user_name = externaltransactions.user_name " +
                                     "and transactions.transaction_time = externaltransactions.transaction_time " +
                                     "WHERE transactions.type = 'IET' and transactions.user_name = ?")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                String tmpResult = "";
                while( rs.next()) {
                    tmpResult += "\n\t\t" + "Amount: " + rs.getString("amount")
                            + " From: " + rs.getString("target_user")
                            + " In: " + rs.getString("target_bank")
                            + " Time: " + rs.getString("transaction_time");
                    result = tmpResult;
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }

    String getOutgoingExternalTransferHistory(String username) {
        String result = "\n\t\tNo transactions.";
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT externaltransactions.target_user,externaltransactions.target_bank," +
                                     "transactions.transaction_time,transactions.amount " +
                                     "FROM transactions JOIN externaltransactions " +
                                     "ON transactions.user_name = externaltransactions.user_name " +
                                     "and transactions.transaction_time = externaltransactions.transaction_time " +
                                     "WHERE transactions.type = 'OET' and transactions.user_name = ?")) {
            preparedStatement.setString(1,username);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                String tmpResult = "";
                while( rs.next()) {
                    tmpResult += "\n\t\t" + "Amount: " + rs.getString("amount")
                            + " To: " + rs.getString("target_user")
                            + " In: " + rs.getString("target_bank")
                            + " Time: " + rs.getString("transaction_time");
                    result = tmpResult;
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return result;
    }

    boolean deposit(String username, String amount) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "UPDATE accounts SET balance = balance + ? WHERE user_name = ?");
             PreparedStatement preparedStatement2 =
                     conn.prepareStatement(
                             "INSERT INTO transactions (user_name,amount,type) VALUES (?,?,?)")) {
            preparedStatement.setInt(1,Integer.valueOf(amount));
            preparedStatement.setString(2,username);
            preparedStatement.executeUpdate();

            preparedStatement2.setString(1,username);
            preparedStatement2.setInt(2,Integer.valueOf(amount));
            preparedStatement2.setString(3,"DP");
            preparedStatement2.executeUpdate();

            conn.commit();

            return true;
        } catch (SQLException e) {
            e.printStackTrace();
            try {
                conn.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
        }
        return false;
    }

    boolean withdraw(String username, String amount) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "UPDATE accounts SET balance = balance - ? WHERE user_name = ?");
             PreparedStatement preparedStatement2 =
                     conn.prepareStatement(
                             "INSERT INTO transactions (user_name,amount,type) VALUES (?,?,?)")) {
            preparedStatement.setInt(1,Integer.valueOf(amount));
            preparedStatement.setString(2,username);
            preparedStatement.executeUpdate();

            preparedStatement2.setString(1,username);
            preparedStatement2.setInt(2,Integer.valueOf(amount));
            preparedStatement2.setString(3,"WD");
            preparedStatement2.executeUpdate();

            conn.commit();

            return true;
        } catch (SQLException e) {
            e.printStackTrace();
            try {
                conn.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
        }
        return false;
    }

    boolean InternalTransfer(String username, String otherUser, String amount) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "UPDATE accounts SET balance = balance - ? WHERE user_name = ?");
             PreparedStatement preparedStatement2 =
                     conn.prepareStatement(
                             "INSERT INTO transactions (user_name,transaction_time,amount,type) VALUES (?,?,?,?)");
             PreparedStatement preparedStatement3 =
                     conn.prepareStatement(
                             "UPDATE accounts SET balance = balance + ? WHERE user_name = ?");
             PreparedStatement preparedStatement4 =
                     conn.prepareStatement(
                             "INSERT INTO internaltransactions (user_name,transaction_time,target_user) " +
                                     "VALUES (?,?,?)")) {

            Timestamp time = new Timestamp(new Date().getTime());

            preparedStatement.setInt(1,Integer.valueOf(amount));
            preparedStatement.setString(2,username);
            preparedStatement.executeUpdate();

            preparedStatement2.setString(1,username);
            preparedStatement2.setTimestamp(2,time);
            preparedStatement2.setInt(3,Integer.valueOf(amount));
            preparedStatement2.setString(4,"IT");
            preparedStatement2.executeUpdate();

            preparedStatement3.setInt(1,Integer.valueOf(amount));
            preparedStatement3.setString(2,otherUser);
            preparedStatement3.executeUpdate();

            preparedStatement4.setString(1,username);
            preparedStatement4.setTimestamp(2,time);
            preparedStatement4.setString(3,otherUser);
            preparedStatement4.executeUpdate();

            conn.commit();

            return true;
        } catch (SQLException e) {
            e.printStackTrace();
            try {
                conn.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
        }
        return false;
    }

    boolean OutgoingExternalTransfer(String username, String otherBank, String otherUser, String amount) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "UPDATE accounts SET balance = balance - ? WHERE user_name = ?");
             PreparedStatement preparedStatement2 =
                     conn.prepareStatement(
                             "INSERT INTO transactions (user_name,transaction_time,amount,type) VALUES (?,?,?,?)");
             PreparedStatement preparedStatement3 =
                     conn.prepareStatement(
                             "INSERT INTO externaltransactions " +
                                     "(user_name,transaction_time,target_user,target_bank) " +
                                     "VALUES (?,?,?,?)")) {

            Timestamp time = new Timestamp(new Date().getTime());

            preparedStatement.setInt(1,Integer.valueOf(amount));
            preparedStatement.setString(2,username);
            preparedStatement.executeUpdate();

            preparedStatement2.setString(1,username);
            preparedStatement2.setTimestamp(2,time);
            preparedStatement2.setInt(3,Integer.valueOf(amount));
            preparedStatement2.setString(4,"OET");
            preparedStatement2.executeUpdate();

            preparedStatement3.setString(1,username);
            preparedStatement3.setTimestamp(2,time);
            preparedStatement3.setString(3,otherUser);
            preparedStatement3.setString(4,otherBank);
            preparedStatement3.executeUpdate();

            conn.commit();

            return true;
        } catch (SQLException e) {
            e.printStackTrace();
            try {
                conn.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
        }
        return false;
    }

    Timestamp IncomingExternalTransfer(String username, String otherBank, String otherUser, String amount) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "UPDATE accounts SET balance = balance + ? WHERE user_name = ?");
             PreparedStatement preparedStatement2 =
                     conn.prepareStatement(
                             "INSERT INTO transactions (user_name,transaction_time,amount,type) VALUES (?,?,?,?)");
             PreparedStatement preparedStatement3 =
                     conn.prepareStatement(
                             "INSERT INTO externaltransactions " +
                                     "(user_name,transaction_time,target_user,target_bank) " +
                                     "VALUES (?,?,?,?)")) {

            Timestamp time = new Timestamp(new Date().getTime());

            preparedStatement.setInt(1,Integer.valueOf(amount));
            preparedStatement.setString(2,username);
            preparedStatement.executeUpdate();

            preparedStatement2.setString(1,username);
            preparedStatement2.setTimestamp(2,time);
            preparedStatement2.setInt(3,Integer.valueOf(amount));
            preparedStatement2.setString(4,"IET");
            preparedStatement2.executeUpdate();

            preparedStatement3.setString(1,username);
            preparedStatement3.setTimestamp(2,time);
            preparedStatement3.setString(3,otherUser);
            preparedStatement3.setString(4,otherBank);
            preparedStatement3.executeUpdate();

            conn.commit();

            return time;
        } catch (SQLException e) {
            e.printStackTrace();
            try {
                conn.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
        }
        return new Timestamp(0);
    }

    String getBankAuth(String otherBank) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT auth_token FROM banks WHERE name = ?")) {
            preparedStatement.setString(1,otherBank);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                if( rs.first()) {
                    return rs.getString("auth_token");
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return "Could not connect to the other bank, try again later.";
    }

    boolean bankAuthenticate(String otherBank, String auth) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "SELECT auth_token FROM banks WHERE name = ? and auth_token = ?")) {
            preparedStatement.setString(1,otherBank);
            preparedStatement.setString(2,auth);
            try (ResultSet rs = preparedStatement.executeQuery()) {
                if(rs.first())
                    return true;
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return false;
    }

    void removeIncomingExternalTransaction(String username, Timestamp time, String amount) {
        try (PreparedStatement preparedStatement =
                     conn.prepareStatement(
                             "UPDATE accounts SET balance = balance - ? WHERE user_name = ?");
             PreparedStatement preparedStatement2 =
                     conn.prepareStatement(
                             "DELETE FROM externaltransactions WHERE user_name = ? and transaction_time = ?");
             PreparedStatement preparedStatement3 =
                     conn.prepareStatement(
                             "DELETE FROM transactions WHERE user_name = ? and transaction_time = ?")) {
            preparedStatement.setInt(1,Integer.valueOf(amount));
            preparedStatement.setString(2,username);
            preparedStatement.executeUpdate();

            preparedStatement2.setString(1,username);
            preparedStatement2.setTimestamp(2,time);
            preparedStatement2.executeUpdate();

            preparedStatement3.setString(1,username);
            preparedStatement3.setTimestamp(2,time);
            preparedStatement3.executeUpdate();

            conn.commit();
        } catch (SQLException e) {
            e.printStackTrace();
            try {
                conn.rollback();
            } catch (SQLException e1) {
                e1.printStackTrace();
            }
        }
    }
}