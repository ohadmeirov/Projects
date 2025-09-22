package com.hit.dao;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

import com.hit.model.Customer;
import com.hit.model.DayOfTheWeek;
import com.hit.model.Restaurant;
import com.hit.model.Table;
import com.hit.model.TimeSlot;

/**
 * SQLite implementation of the Data Access Object.
 * 
 * Database structure:
 * 1. RESTAURANTS table:
 *    - name (PRIMARY KEY)
 *    - opening_hour
 *    - closing_hour
 * 
 * 2. TABLES table:
 *    - id (PRIMARY KEY)
 *    - restaurant_name (FOREIGN KEY to RESTAURANTS)
 *    - capacity
 *    - status (AVAILABLE, RESERVED, etc.)
 * 
 * 3. CUSTOMERS table:
 *    - name (PRIMARY KEY)
 *    - number_of_guests
 *    - day_to_come
 *    - time_to_come
 *    - reserved_table_id
 * 
 * 4. RESERVATIONS table:
 *    - id (PRIMARY KEY AUTOINCREMENT)
 *    - customer_name (FOREIGN KEY to CUSTOMERS)
 *    - restaurant_name (FOREIGN KEY to RESTAURANTS)
 *    - table_id (FOREIGN KEY to TABLES)
 *    - day_of_week
 *    - start_time
 *    - end_time
 *    - status (CONFIRMED, CANCELLED, etc.)
 *    - created_at
 *    - updated_at
 * 
 * 5. WAIT_LIST table:
 *    - id (PRIMARY KEY AUTOINCREMENT)
 *    - customer_name (FOREIGN KEY to CUSTOMERS)
 *    - restaurant_name (FOREIGN KEY to RESTAURANTS)
 *    - day_of_week
 *    - start_time
 *    - end_time
 *    - status (WAITING, NOTIFIED, etc.)
 *    - created_at
 *    - updated_at
 */
public class MyDMSQLiteImpl implements IDao {
    private static final String DB_URL = "jdbc:sqlite:restaurant.db";
    private Connection connection;

    public MyDMSQLiteImpl() {
        initializeDatabase();
    }

    private void initializeDatabase() {
        try {
            connection = DriverManager.getConnection(DB_URL);
            createTables();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    private void createTables() throws SQLException {
        try (Statement stmt = connection.createStatement()) {
            // Create RESTAURANTS table
            stmt.execute("""
                CREATE TABLE IF NOT EXISTS RESTAURANTS (
                    name TEXT PRIMARY KEY,
                    opening_hour INTEGER NOT NULL,
                    closing_hour INTEGER NOT NULL
                )
            """);

            // Create TABLES table
            stmt.execute("""
                CREATE TABLE IF NOT EXISTS TABLES (
                    id INTEGER PRIMARY KEY,
                    restaurant_name TEXT NOT NULL,
                    capacity INTEGER NOT NULL,
                    status TEXT NOT NULL,
                    FOREIGN KEY (restaurant_name) REFERENCES RESTAURANTS(name)
                )
            """);

            // Create CUSTOMERS table
            stmt.execute("""
                CREATE TABLE IF NOT EXISTS CUSTOMERS (
                    name TEXT PRIMARY KEY,
                    number_of_guests INTEGER NOT NULL,
                    day_to_come TEXT NOT NULL,
                    time_to_come TEXT NOT NULL,
                    reserved_table_id INTEGER DEFAULT -1,
                    FOREIGN KEY (reserved_table_id) REFERENCES TABLES(id)
                )
            """);

            // Create RESERVATIONS table
            stmt.execute("""
                CREATE TABLE IF NOT EXISTS RESERVATIONS (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    customer_name TEXT NOT NULL,
                    restaurant_name TEXT NOT NULL,
                    table_id INTEGER NOT NULL,
                    day_of_week TEXT NOT NULL,
                    start_time TEXT NOT NULL,
                    end_time TEXT NOT NULL,
                    status TEXT NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (customer_name) REFERENCES CUSTOMERS(name),
                    FOREIGN KEY (restaurant_name) REFERENCES RESTAURANTS(name),
                    FOREIGN KEY (table_id) REFERENCES TABLES(id)
                )
            """);

            // Create WAIT_LIST table
            stmt.execute("""
                CREATE TABLE IF NOT EXISTS WAIT_LIST (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    customer_name TEXT NOT NULL,
                    restaurant_name TEXT NOT NULL,
                    day_of_week TEXT NOT NULL,
                    start_time TEXT NOT NULL,
                    end_time TEXT NOT NULL,
                    status TEXT NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (customer_name) REFERENCES CUSTOMERS(name),
                    FOREIGN KEY (restaurant_name) REFERENCES RESTAURANTS(name)
                )
            """);
        }
    }

    @Override
    public boolean saveReservation(Customer customer, Restaurant restaurant, Table table,
                                 TimeSlot timeSlot, DayOfTheWeek dayOfWeek) {
        // First ensure all entities exist in their respective tables
        if (!ensureEntityExists(customer, "CUSTOMERS") ||
            !ensureEntityExists(restaurant, "RESTAURANTS") ||
            !ensureEntityExists(table, "TABLES")) {
            return false;
        }

        String sql = """
            INSERT INTO RESERVATIONS (customer_name, restaurant_name, table_id, day_of_week, 
            start_time, end_time, status)
            VALUES (?, ?, ?, ?, ?, ?, ?)
        """;

        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, customer.getName());
            pstmt.setString(2, restaurant.getName());
            pstmt.setInt(3, table.getId());
            pstmt.setString(4, dayOfWeek.toString());
            pstmt.setString(5, timeSlot.getTime().toString());
            pstmt.setString(6, timeSlot.getTime().toString());
            pstmt.setString(7, "CONFIRMED");
            int rowsAffected = pstmt.executeUpdate();
            if (rowsAffected == 0) {
                System.err.println("Failed to save reservation - No rows affected.");
            }
            return rowsAffected > 0;
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }

    @Override
    public boolean deleteReservation(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                                   DayOfTheWeek dayOfWeek) {
        String sql = """
            DELETE FROM RESERVATIONS 
            WHERE customer_name = ? AND restaurant_name = ? 
            AND day_of_week = ? AND start_time = ? AND end_time = ?
        """;

        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, customer.getName());
            pstmt.setString(2, restaurant.getName());
            pstmt.setString(3, dayOfWeek.toString());
            pstmt.setString(4, timeSlot.getTime().toString());
            pstmt.setString(5, timeSlot.getTime().toString());
            
            int rowsAffected = pstmt.executeUpdate();
            System.out.println("Rows affected in deleteReservation: " + rowsAffected);
            return rowsAffected > 0;
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }

    @Override
    public boolean addToWaitList(Customer customer, Restaurant restaurant, TimeSlot timeSlot,
                                DayOfTheWeek dayOfWeek) {
        // First ensure customer and restaurant exist
        if (!ensureEntityExists(customer, "CUSTOMERS") ||
            !ensureEntityExists(restaurant, "RESTAURANTS")) {
            return false;
        }

        // Check if already in waitlist for same customer, restaurant, day, time, and number of guests
        String checkSql = """
            SELECT COUNT(*) FROM WAIT_LIST
            WHERE customer_name = ? AND restaurant_name = ? AND day_of_week = ? AND start_time = ? AND end_time = ? AND status = 'WAITING'
        """;
        try (PreparedStatement checkStmt = connection.prepareStatement(checkSql)) {
            checkStmt.setString(1, customer.getName());
            checkStmt.setString(2, restaurant.getName());
            checkStmt.setString(3, dayOfWeek.toString());
            checkStmt.setString(4, timeSlot.getTime().toString());
            checkStmt.setString(5, timeSlot.getTime().toString());
            ResultSet rs = checkStmt.executeQuery();
            if (rs.next() && rs.getInt(1) > 0) {
                System.out.println("Customer already in waitlist for this slot.");
                return false;
            }
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }

        String sql = """
            INSERT INTO WAIT_LIST (customer_name, restaurant_name, day_of_week, 
            start_time, end_time, status)
            VALUES (?, ?, ?, ?, ?, ?)
        """;

        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, customer.getName());
            pstmt.setString(2, restaurant.getName());
            pstmt.setString(3, dayOfWeek.toString());
            pstmt.setString(4, timeSlot.getTime().toString());
            pstmt.setString(5, timeSlot.getTime().toString());
            pstmt.setString(6, "WAITING");
            int rowsAffected = pstmt.executeUpdate();
            if (rowsAffected == 0) {
                System.err.println("Failed to add to waitlist - No rows affected.");
            }
            return rowsAffected > 0;
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }

    @Override
    public List<TimeSlot> getCustomerReservations(Customer customer) {
        String sql = "SELECT day_of_week, start_time, end_time FROM RESERVATIONS WHERE customer_name = ?";
        List<TimeSlot> reservations = new ArrayList<>();
        
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, customer.getName());
            ResultSet rs = pstmt.executeQuery();
            while (rs.next()) {
                DayOfTheWeek day = DayOfTheWeek.valueOf(rs.getString("day_of_week"));
                LocalTime time = LocalTime.parse(rs.getString("start_time"));
                reservations.add(new TimeSlot(day, time));
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return reservations;
    }

    @Override
    public List<TimeSlot> getRestaurantReservations(Restaurant restaurant, DayOfTheWeek dayOfWeek) {
        String sql = "SELECT start_time, end_time FROM RESERVATIONS WHERE restaurant_name = ? AND day_of_week = ?";
        List<TimeSlot> reservations = new ArrayList<>();
        
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, restaurant.getName());
            pstmt.setString(2, dayOfWeek.toString());
            ResultSet rs = pstmt.executeQuery();
            while (rs.next()) {
                LocalTime time = LocalTime.parse(rs.getString("start_time"));
                reservations.add(new TimeSlot(dayOfWeek, time));
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return reservations;
    }

    public List<ReservationRecord> getAllReservationsForRestaurant(Restaurant restaurant) {
        List<ReservationRecord> records = new ArrayList<>();
        String sql = "SELECT r.table_id, r.day_of_week, r.start_time, c.name, c.number_of_guests, c.day_to_come, c.time_to_come " +
                     "FROM RESERVATIONS r JOIN CUSTOMERS c ON r.customer_name = c.name " +
                     "WHERE r.restaurant_name = ?";
        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            pstmt.setString(1, restaurant.getName());
            ResultSet rs = pstmt.executeQuery();
            while (rs.next()) {
                int tableId = rs.getInt("table_id");
                DayOfTheWeek day = DayOfTheWeek.valueOf(rs.getString("day_of_week"));
                LocalTime time = LocalTime.parse(rs.getString("start_time"));
                String customerName = rs.getString("name");
                int guests = rs.getInt("number_of_guests");
                DayOfTheWeek custDay = DayOfTheWeek.valueOf(rs.getString("day_to_come"));
                LocalTime custTime = LocalTime.parse(rs.getString("time_to_come"));
                ReservationRecord rec = new ReservationRecord(tableId, day, time, customerName, guests, custDay, custTime);
                records.add(rec);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return records;
    }

    // Helper class for reservation info
    public static class ReservationRecord {
        public final int tableId;
        public final DayOfTheWeek day;
        public final LocalTime time;
        public final String customerName;
        public final int guests;
        public final DayOfTheWeek custDay;
        public final LocalTime custTime;
        public ReservationRecord(int tableId, DayOfTheWeek day, LocalTime time, String customerName, int guests, DayOfTheWeek custDay, LocalTime custTime) {
            this.tableId = tableId;
            this.day = day;
            this.time = time;
            this.customerName = customerName;
            this.guests = guests;
            this.custDay = custDay;
            this.custTime = custTime;
        }
    }

    private boolean ensureEntityExists(Object entity, String tableName, String... restaurantName) {
        String sql;
        if (entity instanceof Customer) {
            sql = "SELECT COUNT(*) FROM " + tableName + " WHERE name = ?";
        } else if (entity instanceof Restaurant) {
            sql = "SELECT COUNT(*) FROM " + tableName + " WHERE name = ?";
        } else if (entity instanceof Table) {
            sql = "SELECT COUNT(*) FROM " + tableName + " WHERE id = ?";
        } else {
            throw new IllegalArgumentException("Unsupported entity type: " + entity.getClass().getName());
        }

        try (PreparedStatement pstmt = connection.prepareStatement(sql)) {
            if (entity instanceof Customer) {
                pstmt.setString(1, ((Customer) entity).getName());
            } else if (entity instanceof Restaurant) {
                pstmt.setString(1, ((Restaurant) entity).getName());
            } else if (entity instanceof Table) {
                pstmt.setInt(1, ((Table) entity).getId());
            }

            ResultSet rs = pstmt.executeQuery();
            if (rs.next() && rs.getInt(1) > 0) {
                return true;
            }
            // Entity doesn't exist, insert it
            return insertEntity(entity, tableName, restaurantName);
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }

    private boolean insertEntity(Object entity, String tableName, String... restaurantName) {
        try (PreparedStatement pstmt = connection.prepareStatement(getInsertSQL(entity, tableName))) {
            setEntityParameters(pstmt, entity, restaurantName);
            return pstmt.executeUpdate() > 0;
        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        }
    }

    private String getInsertSQL(Object entity, String tableName) {
        if (entity instanceof Customer) {
            return "INSERT INTO CUSTOMERS (name, number_of_guests, day_to_come, time_to_come, reserved_table_id) VALUES (?, ?, ?, ?, ?)";
        } else if (entity instanceof Restaurant) {
            return "INSERT INTO RESTAURANTS (name, opening_hour, closing_hour) VALUES (?, ?, ?)";
        } else if (entity instanceof Table) {
            return "INSERT INTO TABLES (id, restaurant_name, capacity, status) VALUES (?, ?, ?, ?)";
        }
        throw new IllegalArgumentException("Unsupported entity type: " + entity.getClass().getName());
    }

    private void setEntityParameters(PreparedStatement pstmt, Object entity, String... restaurantName) throws SQLException {
        if (entity instanceof Customer) {
            Customer customer = (Customer) entity;
            pstmt.setString(1, customer.getName());
            pstmt.setInt(2, customer.getNumberOfGuests());
            pstmt.setString(3, customer.getDayToCome().toString());
            pstmt.setString(4, customer.getTimeToCome().toString());
            pstmt.setInt(5, customer.getReservedTableId());
        } else if (entity instanceof Restaurant) {
            Restaurant restaurant = (Restaurant) entity;
            pstmt.setString(1, restaurant.getName());
            pstmt.setInt(2, restaurant.getOpeningHour());
            pstmt.setInt(3, restaurant.getClosingHour());
        } else if (entity instanceof Table) {
            Table table = (Table) entity;
            pstmt.setInt(1, table.getId());
            pstmt.setString(2, restaurantName.length > 0 ? restaurantName[0] : "");
            pstmt.setInt(3, table.getCapacity());
            pstmt.setString(4, table.getStatus().toString());
        }
    }
}