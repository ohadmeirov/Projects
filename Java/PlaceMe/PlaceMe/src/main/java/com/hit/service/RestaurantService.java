package com.hit.service;

import com.hit.dao.MyDMSQLiteImpl;
import com.hit.model.Customer;
import com.hit.model.Restaurant;
import com.hit.model.Table;
import com.hit.model.TableStatus;

import java.sql.*;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class RestaurantService {

    private List<Table> tables;

    // Returns a default Restaurant. In a real implementation, this could fetch from DB.
    public Restaurant getDefaultRestaurant() {
        // For now, return a stub Restaurant (with dummy values)
        tables = new ArrayList<>();
        return new Restaurant("Default Restaurant", tables, 8, 22);
    }

    // SET operations
    public void setName(Restaurant restaurant, String newName) {
        restaurant.setName(newName);
    }

    public void setOpeningHour(Restaurant restaurant, int openingHour) {
        restaurant.setOpeningHour(openingHour);
    }

    public void setClosingHour(Restaurant restaurant, int closingHour) {
        restaurant.setClosingHour(closingHour);
    }

    // GET operations
    public String getName(Restaurant restaurant) {
        return restaurant.getName();
    }

    public int getOpeningHour(Restaurant restaurant) { return restaurant.getOpeningHour(); }

    public int getClosingHour(Restaurant restaurant) {
        return restaurant.getClosingHour();
    }

    // Returns a real Restaurant from DB by name (with tables, etc.)
    public Restaurant getRestaurantByName(String name) {
        String dbUrl = "jdbc:sqlite:restaurant.db";
        Restaurant restaurant = null;
        List<Table> tables = new ArrayList<>();
        int openingHour = 0;
        int closingHour = 0;

        try (Connection conn = DriverManager.getConnection(dbUrl)) {
            // Fetch restaurant info
            String restSql = "SELECT opening_hour, closing_hour FROM RESTAURANTS WHERE name = ?";
            try (PreparedStatement pstmt = conn.prepareStatement(restSql)) {
                pstmt.setString(1, name);
                try (ResultSet rs = pstmt.executeQuery()) {
                    if (rs.next()) {
                        openingHour = rs.getInt("opening_hour");
                        closingHour = rs.getInt("closing_hour");
                    } else {
                        // Restaurant not found
                        return null;
                    }
                }
            }

            // Fetch tables for this restaurant
            String tableSql = "SELECT id, capacity, status FROM TABLES WHERE restaurant_name = ?";
            try (PreparedStatement pstmt = conn.prepareStatement(tableSql)) {
                pstmt.setString(1, name);
                try (ResultSet rs = pstmt.executeQuery()) {
                    while (rs.next()) {
                        int id = rs.getInt("id");
                        int capacity = rs.getInt("capacity");
                        String statusStr = rs.getString("status");
                        TableStatus status = TableStatus.valueOf(statusStr);
                        Table table = new Table(id, capacity);
                        table.setStatus(status);
                        tables.add(table);
                    }
                }
            }

            // Print all tables loaded for debug
            System.out.println("Tables loaded for restaurant " + name + ":");
            for (Table t : tables) {
                System.out.println("Table id=" + t.getId() + ", capacity=" + t.getCapacity() + ", status=" + t.getStatus());
            }

            restaurant = new Restaurant(name, tables, openingHour, closingHour);

            // Initialize TimeSlotManager with all reservations from DB
            MyDMSQLiteImpl dao = new MyDMSQLiteImpl();
            Set<String> reservationKeys = new HashSet<>();
            for (MyDMSQLiteImpl.ReservationRecord rec : dao.getAllReservationsForRestaurant(restaurant)) {
                String key = rec.tableId + "|" + rec.day + "|" + rec.time;
                if (reservationKeys.contains(key)) {
                    System.out.println("DUPLICATE reservation for table " + rec.tableId + " at " + rec.day + " " + rec.time);
                    continue;
                }
                reservationKeys.add(key);

                Table table = tables.stream().filter(t -> t.getId() == rec.tableId).findFirst().orElse(null);
                if (table == null) {
                    System.out.println("WARNING: Table id " + rec.tableId + " not found in restaurant " + name);
                    continue;
                }
                Customer customer = new Customer(rec.customerName, rec.guests, rec.custDay, rec.custTime);
                customer.setReservedTableId(table.getId());
                restaurant.getTimeSlotManager().reserveTable(
                    restaurant, table, customer, rec.day, rec.time
                );
                System.out.println("After init reservation: Table id=" + table.getId() + ", status=" + table.getStatus() +
                    ", day=" + rec.day + ", time=" + rec.time + ", customer=" + rec.customerName);
            }
        } catch (SQLException e) {
            e.printStackTrace();
            return null;
        }

        return restaurant;
    }
}