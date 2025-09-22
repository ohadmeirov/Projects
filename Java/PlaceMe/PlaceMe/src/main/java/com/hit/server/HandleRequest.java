package com.hit.server;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import com.hit.controller.ControllerFactory;
import com.hit.model.Customer;
import com.hit.model.ReservationRequest;
import com.hit.model.Restaurant;
import com.hit.model.Table;

import java.io.*;
import java.net.Socket;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.HashMap;
import java.util.Map;

public class HandleRequest implements Runnable {
    private Socket clientSocket;
    private ControllerFactory controllerFactory;
    private Gson gson;

    public HandleRequest(Socket clientSocket) {
        this.clientSocket = clientSocket;
        this.controllerFactory = ControllerFactory.getInstance();
        
        // Configure Gson with custom type adapters and use @Expose annotation
        this.gson = new GsonBuilder()
            .excludeFieldsWithModifiers(java.lang.reflect.Modifier.TRANSIENT)
            .registerTypeAdapter(LocalTime.class, new TypeAdapter<LocalTime>() {
                @Override
                public void write(JsonWriter out, LocalTime value) throws IOException {
                    if (value == null) {
                        out.nullValue();
                    } else {
                        out.value(value.format(DateTimeFormatter.ISO_LOCAL_TIME));
                    }
                }

                @Override
                public LocalTime read(JsonReader in) throws IOException {
                    String timeStr = in.nextString();
                    return timeStr == null ? null : LocalTime.parse(timeStr, DateTimeFormatter.ISO_LOCAL_TIME);
                }
            })
            .create();
    }

    @Override
    public void run() {
        try (
            BufferedReader in = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
            PrintWriter out = new PrintWriter(clientSocket.getOutputStream(), true)
        ) {
            String requestLine = in.readLine();
            if (requestLine == null) {
                return;
            }
            System.out.println("Received request line: " + requestLine);
            
            // Parse HTTP request
            String[] requestParts = requestLine.split(" ");
            if (requestParts.length < 3) {
                sendErrorResponse(out, "Invalid request format");
                return;
            }
            
            String method = requestParts[0];
            String path = requestParts[1];
            
            // Read headers
            Map<String, String> headers = new HashMap<>();
            String header;
            int contentLength = 0;
            while ((header = in.readLine()) != null && !header.isEmpty()) {
                System.out.println("Received header: " + header);
                String[] parts = header.split(": ", 2);
                if (parts.length == 2) {
                    headers.put(parts[0], parts[1]);
                    if (parts[0].equalsIgnoreCase("Content-Length")) {
                        contentLength = Integer.parseInt(parts[1]);
                    }
                }
            }
            
            /*// Handle static files
            if (path.equals("/") || path.equals("/index.html")) {
                serveStaticFile(out, "index.html", "text/html");
                return;
            } else if (path.equals("/favicon.ico")) {
                serveStaticFile(out, "favicon.ico", "image/x-icon");
                return;
            }*/
            
            // Read body if exists
            String body = null;
            if (contentLength > 0) {
                char[] buffer = new char[contentLength];
                in.read(buffer, 0, contentLength);
                body = new String(buffer);
                System.out.println("Received body: " + body);
            }
            
            // Parse path
            String[] pathParts = path.split("/");
            if (pathParts.length < 2) {
                sendErrorResponse(out, "Invalid path format");
                return;
            }
            String controllerPath = pathParts[1];
            String action = pathParts.length > 2 ? pathParts[2] : "";
            String fullPath = controllerPath + (action.isEmpty() ? "" : "/" + action);
            
            System.out.println("Parsed path: " + fullPath);
            System.out.println("Sending request to controller with path: " + fullPath);
            
            // Create request object with parsed body
            Object requestBody = null;
            if (body != null && !body.isEmpty()) {
                try {
                    switch (controllerPath) {
                        case "reservation":
                            requestBody = gson.fromJson(body, ReservationRequest.class);
                            break;
                        case "restaurant":
                            requestBody = gson.fromJson(body, Restaurant.class);
                            break;
                        case "table":
                            requestBody = gson.fromJson(body, Table.class);
                            break;
                        case "customer":
                            requestBody = gson.fromJson(body, Customer.class);
                            break;
                    }
                } catch (Exception e) {
                    sendErrorResponse(out, "Invalid request body format: " + e.getMessage());
                    return;
                }
            }
            
            Request.Headers reqHeaders = new Request.Headers();
            reqHeaders.set("method", method);
            reqHeaders.set("action", fullPath);
            Request<Object> request = new Request<>(reqHeaders, requestBody);
            
            // Handle request
            Response<?> response = controllerFactory.handleRequest(fullPath, request);
            
            // Send response
            String jsonResponse = gson.toJson(response);
            System.out.println("Sending response: " + jsonResponse);
            out.println("HTTP/1.1 200 OK");
            out.println("Content-Type: application/json");
            out.println("Content-Length: " + jsonResponse.getBytes().length);
            out.println();
            out.println(jsonResponse);
            
        } catch (Exception e) {
            System.err.println("Error handling request: " + e.getMessage());
            e.printStackTrace();
            try {
                PrintWriter out = new PrintWriter(clientSocket.getOutputStream(), true);
                sendErrorResponse(out, "Internal server error: " + e.getMessage());
            } catch (IOException ex) {
                ex.printStackTrace();
            }
        } finally {
            try {
                clientSocket.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
    
    private void sendErrorResponse(PrintWriter out, String errorMessage) {
        Response<?> errorResponse = new Response<>(false, errorMessage, null);
        String jsonResponse = gson.toJson(errorResponse);
        out.println("HTTP/1.1 400 Bad Request");
        out.println("Content-Type: application/json");
        out.println("Content-Length: " + jsonResponse.getBytes().length);
        out.println();
        out.println(jsonResponse);
    }
    
    private void serveStaticFile(PrintWriter out, String filename, String contentType) throws IOException {
        Path filePath = Paths.get("src/main/resources", filename);
        if (!Files.exists(filePath)) {
            sendErrorResponse(out, "File not found: " + filename);
            return;
        }
        
        byte[] fileContent = Files.readAllBytes(filePath);
        out.println("HTTP/1.1 200 OK");
        out.println("Content-Type: " + contentType);
        out.println("Content-Length: " + fileContent.length);
        out.println();
        out.flush();
        
        OutputStream outputStream = clientSocket.getOutputStream();
        outputStream.write(fileContent);
        outputStream.flush();
    }
}