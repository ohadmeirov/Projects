package com.hit.controller;

import com.hit.dao.MyDMSQLiteImpl;
import com.hit.model.StrategyType;
import com.hit.server.Request;
import com.hit.server.Response;
import com.hit.service.*;

import java.util.HashMap;
import java.util.Map;

public class ControllerFactory {
    private static ControllerFactory instance;
    private Map<String, Controller> controllers;

    private ControllerFactory() {
        controllers = new HashMap<>();
        
        // Initialize services
        MyDMSQLiteImpl dao = new MyDMSQLiteImpl();
        TableReservationServiceImpl reservationService = new TableReservationServiceImpl(dao, StrategyType.OPTIMIZED);
        RestaurantService restaurantService = new RestaurantService();
        TableService tableService = new TableService();
        CustomerService customerService = new CustomerService();
        
        // Register controllers
        controllers.put("reservation", new ReservationController(reservationService));
        controllers.put("restaurant", new RestaurantController(restaurantService));
        controllers.put("table", new TableController(tableService));
        controllers.put("customer", new CustomerController(customerService));
        
        System.out.println("Registered controllers: " + controllers.keySet());
    }

    public static synchronized ControllerFactory getInstance() {
        if (instance == null) {
            instance = new ControllerFactory();
        }
        return instance;
    }

    public Response<?> handleRequest(String action, Request<?> request) {
        System.out.println("Handling request for action: " + action);
        String controllerName = action.split("/")[0];
        System.out.println("Looking for controller: " + controllerName);
        
        Controller controller = controllers.get(controllerName);
        if (controller == null) {
            System.out.println("Controller not found: " + controllerName);
            return new Response<>(false, "Controller not found: " + controllerName, null);
        }

        System.out.println("Found controller: " + controllerName);
        return controller.handleRequest(action, request);
    }

    public void registerController(String name, Controller controller) {
        controllers.put(name, controller);
    }
} 