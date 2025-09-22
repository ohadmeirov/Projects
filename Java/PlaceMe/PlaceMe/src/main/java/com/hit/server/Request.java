package com.hit.server;

import java.util.HashMap;
import java.util.Map;

public class Request<T> {
    private Headers headers;
    private T body;

    public Request() {
        this.headers = new Headers();
    }

    public Request(Headers headers, T body) {
        this.headers = headers;
        this.body = body;
    }

    public Headers getHeaders() {
        return headers;
    }

    public void setHeaders(Headers headers) {
        this.headers = headers;
    }

    public T getBody() {
        return body;
    }

    public void setBody(T body) {
        this.body = body;
    }

    public static class Headers {
        private Map<String, String> headers;

        public Headers() {
            this.headers = new HashMap<>();
        }

        public void set(String key, String value) {
            headers.put(key, value);
        }

        public String get(String key) {
            return headers.get(key);
        }

        public Map<String, String> getAll() {
            return headers;
        }
    }
}
