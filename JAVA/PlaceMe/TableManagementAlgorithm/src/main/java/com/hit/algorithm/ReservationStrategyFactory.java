package com.hit.algorithm;

import com.hit.model.StrategyType;

// מחלקה זו משמשת כ-Factory ליצירת מופע של אסטרטגיית הזמנת שולחן לפי סוג (נאיבי/אופטימלי)
public class ReservationStrategyFactory {
    // מחזירה מופע של אסטרטגיה לפי סוג ("naive" או "optimized")
    public static IAlgoTableReservationStrategy createStrategy(StrategyType type) {
        switch (type) {
            case OPTIMIZED:
                return new OptimizedTableReservationStrategy();
            case NAIVE:
                return new NaiveTableReservationStrategy();
            default:
                throw new IllegalArgumentException("Unknown strategy type: " + type);
        }
    }
} 