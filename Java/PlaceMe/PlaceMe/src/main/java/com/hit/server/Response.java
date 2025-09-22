package com.hit.server;

import com.google.gson.annotations.Expose;

public class Response<T> {
    @Expose
    private boolean success;
    @Expose
    private String message;
    @Expose
    private T data;

    public Response() {
    }

    public Response(boolean success, String message, T data) {
        this.success = success;
        this.message = message;
        this.data = data;
    }

    public boolean isSuccess() {
        return success;
    }

    public void setSuccess(boolean success) {
        this.success = success;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public T getData() {
        return data;
    }

    public void setData(T data) {
        this.data = data;
    }

    public static <T> Response<T> error(String message) {
        return new Response<>(false, message, null);
    }
}