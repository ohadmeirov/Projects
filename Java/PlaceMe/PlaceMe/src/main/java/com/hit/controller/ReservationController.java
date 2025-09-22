package com.hit.controller;

import com.hit.model.*;
import com.hit.server.Request;
import com.hit.server.Response;
import com.hit.service.TableReservationServiceImpl;

import java.util.List;
import java.util.Set;

public class ReservationController implements Controller {
    private final TableReservationServiceImpl reservationService;

    public ReservationController(TableReservationServiceImpl reservationService) {
        this.reservationService = reservationService;
    }

    @Override
    public Response<?> handleRequest(String action, Request<?> request) {
        String[] parts = action.split("/");
        if (parts.length < 2) {
            return new Response<>(false, "Invalid action format", null);
        }

        String operation = parts[1];
        switch (operation) {
            case "reserve":
                return handleReserve(request);
            case "cancel":
                return handleCancel(request);
            case "waitlist":
                return handleWaitlist(request);
            case "getAvailableTables":
                return handleGetAvailableTables(request);
            case "getCustomerReservations":
                return handleGetCustomerReservations(request);
            case "getRestaurantReservations":
                return handleGetRestaurantReservations(request);
            default:
                return new Response<>(false, "Unknown operation: " + operation, null);
        }
    }

    private Response<?> handleReserve(Request<?> request) {
        try {
            ReservationRequest reservationRequest = (ReservationRequest) request.getBody();
            System.out.println("\n=== RESERVATION REQUEST ===");
            System.out.println("Customer: " + reservationRequest.getCustomer());
            System.out.println("Restaurant: " + reservationRequest.getRestaurant());
            System.out.println("Time Slot: " + reservationRequest.getTimeSlot());
            System.out.println("Day: " + reservationRequest.getDayOfWeek());
            System.out.println("Strategy: " + reservationRequest.getStrategyType());

            Table table = reservationService.reserveTable(
                reservationRequest.getCustomer(),
                reservationRequest.getRestaurant(),
                reservationRequest.getTimeSlot(),
                reservationRequest.getDayOfWeek(),
                reservationRequest.getStrategyType()
            );

            if (table != null) {
                System.out.println("Reservation successful! Table ID: " + table.getId());
                return new Response<>(true, "Reservation successful! Table ID: " + table.getId(), table);
            } else {
                System.out.println("Reservation failed - No suitable table found for the specified time and day.");
                return new Response<>(false, "Reservation failed - No suitable table found for the specified time and day.", null);
            }
        } catch (Exception e) {
            return new Response<>(false, "Failed to reserve table: " + e.getMessage(), null);
        }
    }

    private Response<?> handleCancel(Request<?> request) {
        try {
            ReservationRequest reservationRequest = (ReservationRequest) request.getBody();
            System.out.println("\n=== CANCELLATION REQUEST ===");
            System.out.println("Customer: " + reservationRequest.getCustomer());
            System.out.println("Restaurant: " + reservationRequest.getRestaurant());
            System.out.println("Time Slot: " + reservationRequest.getTimeSlot());
            System.out.println("Day: " + reservationRequest.getDayOfWeek());

            boolean success = reservationService.cancelReservation(
                reservationRequest.getCustomer(),
                reservationRequest.getRestaurant(),
                reservationRequest.getTimeSlot(),
                reservationRequest.getDayOfWeek()
            );

            if (success) {
                System.out.println("Cancellation successful!");
                return new Response<>(true, "Cancellation successful!", null);
            } else {
                System.out.println("Cancellation failed - Reservation not found or could not be cancelled");
                return new Response<>(false, "Cancellation failed - Reservation not found or could not be cancelled", null);
            }
        } catch (Exception e) {
            return new Response<>(false, "Failed to cancel reservation: " + e.getMessage(), null);
        }
    }

    private Response<?> handleWaitlist(Request<?> request) {
        try {
            ReservationRequest reservationRequest = (ReservationRequest) request.getBody();
            System.out.println("\n=== WAITLIST REQUEST ===");
            System.out.println("Customer: " + reservationRequest.getCustomer());
            System.out.println("Restaurant: " + reservationRequest.getRestaurant());
            System.out.println("Time Slot: " + reservationRequest.getTimeSlot());
            System.out.println("Day: " + reservationRequest.getDayOfWeek());

            boolean success = reservationService.joinToWaitlist(
                reservationRequest.getCustomer(),
                reservationRequest.getRestaurant(),
                reservationRequest.getTimeSlot(),
                reservationRequest.getDayOfWeek()
            );

            if (success) {
                System.out.println("Successfully joined waitlist!");
                return new Response<>(true, "Successfully joined waitlist!", null);
            } else {
                System.out.println("Failed to join waitlist - No suitable table found or no existing reservation in time window.");
                return new Response<>(false, "Failed to join waitlist - No suitable table found or no existing reservation in time window.", null);
            }
        } catch (Exception e) {
            return new Response<>(false, "Failed to add to waitlist: " + e.getMessage(), null);
        }
    }

    private Response<?> handleGetAvailableTables(Request<?> request) {
        try {
            ReservationRequest reservationRequest = (ReservationRequest) request.getBody();
            System.out.println("\n=== GET AVAILABLE TABLES REQUEST ===");
            System.out.println("Restaurant: " + reservationRequest.getRestaurant());
            System.out.println("Time Slot: " + reservationRequest.getTimeSlot());
            System.out.println("Day: " + reservationRequest.getDayOfWeek());

            Set<Table> availableTables = reservationService.getAvailableTables(
                reservationRequest.getRestaurant(),
                reservationRequest.getTimeSlot(),
                reservationRequest.getDayOfWeek()
            );

            return new Response<>(true, "Available tables retrieved successfully", availableTables);
        } catch (Exception e) {
            return new Response<>(false, "Failed to retrieve available tables: " + e.getMessage(), null);
        }
    }

    private Response<?> handleGetCustomerReservations(Request<?> request) {
        try {
            Customer customer = (Customer) request.getBody();
            System.out.println("\n=== GET CUSTOMER RESERVATIONS REQUEST ===");
            System.out.println("Customer: " + customer);

            List<TimeSlot> customerReservations = reservationService.getCustomerReservations(customer);

            return new Response<>(true, "Customer reservations retrieved successfully", customerReservations);
        } catch (Exception e) {
            return new Response<>(false, "Failed to retrieve customer reservations: " + e.getMessage(), null);
        }
    }

    private Response<?> handleGetRestaurantReservations(Request<?> request) {
        try {
            Restaurant restaurant = (Restaurant) request.getBody();
            DayOfTheWeek dayOfWeek = (DayOfTheWeek) request.getBody();
            System.out.println("\n=== GET RESTAURANT RESERVATIONS REQUEST ===");
            System.out.println("Restaurant: " + restaurant);
            System.out.println("Day: " + dayOfWeek);

            List<TimeSlot> restaurantReservations = reservationService.getRestaurantReservations(restaurant, dayOfWeek);

            return new Response<>(true, "Restaurant reservations retrieved successfully", restaurantReservations);
        } catch (Exception e) {
            return new Response<>(false, "Failed to retrieve restaurant reservations: " + e.getMessage(), null);
        }
    }
}