package com.hit.controller;

import com.hit.model.Table;
import com.hit.server.Request;
import com.hit.server.Response;
import com.hit.service.TableService;

public class TableController implements Controller {
    private final TableService tableService;

    public TableController(TableService tableService) {
        this.tableService = tableService;
    }

    @Override
    public Response<?> handleRequest(String action, Request<?> request) {
        String[] parts = action.split("/");
        if (parts.length < 2) {
            return new Response<>(false, "Invalid action format", null);
        }

        String operation = parts[1];
        switch (operation) {
            case "get":
                return handleGetTable(request);
            case "update":
                return handleUpdateTable(request);
            default:
                return new Response<>(false, "Unknown operation: " + operation, null);
        }
    }

    private Response<?> handleGetTable(Request<?> request) {
        try {
            Table table = (Table) request.getBody();
            return new Response<>(true, "Table retrieved successfully", table);
        } catch (Exception e) {
            return new Response<>(false, "Failed to get table: " + e.getMessage(), null);
        }
    }

    private Response<?> handleUpdateTable(Request<?> request) {
        try {
            Table table = (Table) request.getBody();
            // Add table update logic here
            return new Response<>(true, "Table updated successfully", table);
        } catch (Exception e) {
            return new Response<>(false, "Failed to update table: " + e.getMessage(), null);
        }
    }
} 