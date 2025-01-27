package com.example.MyTrip.fragments.continentsFragments;

import android.graphics.Color;
import android.graphics.drawable.GradientDrawable;
import android.util.Log;
import android.view.Gravity;
import android.view.ViewGroup;
import android.widget.Button;
import androidx.annotation.NonNull;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.RecyclerView;
import android.animation.StateListAnimator;
import android.content.res.ColorStateList;
import android.graphics.Typeface;
import android.graphics.drawable.RippleDrawable;
import android.view.MotionEvent;
import com.example.MyTrip.R;

public class ContinentsAdapter extends RecyclerView.Adapter<ContinentsAdapter.ViewHolder> {

    private final String[] continents = {
            "Europe", "Asia", "Africa", "North America", "South America", "Australia"
    };

    private final int[] backgroundColors = {
            Color.parseColor("#4C6FFF"), // Modern blue for Europe
            Color.parseColor("#FF6B6B"), // Red-orange for Asia
            Color.parseColor("#4ECDC4"), // Turquoise for Africa
            Color.parseColor("#9C6ADE"), // Purple for North America
            Color.parseColor("#FF9F43"), // Orange for South America
            Color.parseColor("#45AAF2")  // Light blue for Australia
    };

    private final int[] highlightColors = {
            Color.parseColor("#3557E8"),
            Color.parseColor("#E84C4C"),
            Color.parseColor("#36B5AE"),
            Color.parseColor("#8656C3"),
            Color.parseColor("#E88C31"),
            Color.parseColor("#3496DB")
    };

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        Button button = new Button(parent.getContext());

        // Enhanced Layout settings
        ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MATCH_PARENT,
                250  // Slightly increased height
        );
        button.setLayoutParams(layoutParams);

        // Improved margins
        ViewGroup.MarginLayoutParams marginLayoutParams = new ViewGroup.MarginLayoutParams(layoutParams);
        marginLayoutParams.setMargins(40, 25, 40, 25);
        button.setLayoutParams(marginLayoutParams);

        // Enhanced text styling
        button.setTextSize(28);
        button.setTypeface(Typeface.DEFAULT_BOLD);
        button.setTextColor(Color.WHITE);
        button.setGravity(Gravity.CENTER);
        button.setAllCaps(false);

        // Add 3D elevation effect
        button.setElevation(12);
        button.setStateListAnimator(null); // Prevents default animation

        // Create advanced background
        GradientDrawable drawable = new GradientDrawable();
        drawable.setShape(GradientDrawable.RECTANGLE);
        drawable.setCornerRadius(35);

        // Add gradient
        drawable.setGradientType(GradientDrawable.LINEAR_GRADIENT);
        drawable.setOrientation(GradientDrawable.Orientation.TL_BR);

        // Add ripple effect for clicks
        ColorStateList colorStateList = new ColorStateList(
                new int[][]{
                        new int[]{android.R.attr.state_pressed},
                        new int[]{}
                },
                new int[]{
                        Color.parseColor("#20FFFFFF"),
                        Color.TRANSPARENT
                }
        );

        RippleDrawable rippleDrawable = new RippleDrawable(colorStateList, drawable, null);
        button.setBackground(rippleDrawable);

        // Add click animation
        button.setOnTouchListener((v, event) -> {
            switch (event.getAction()) {
                case MotionEvent.ACTION_DOWN:
                    v.animate().scaleX(0.95f).scaleY(0.95f).setDuration(100).start();
                    break;
                case MotionEvent.ACTION_UP:
                case MotionEvent.ACTION_CANCEL:
                    v.animate().scaleX(1f).scaleY(1f).setDuration(100).start();
                    break;
            }
            return false;
        });

        return new ViewHolder(button);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Log.d("ContinentsAdapter", "onBindViewHolder: Binding data for position " + position);
        Button button = (Button) holder.itemView;
        button.setText(continents[position]);

        // Set gradient colors based on position
        GradientDrawable drawable = new GradientDrawable(
                GradientDrawable.Orientation.TL_BR,
                new int[]{backgroundColors[position], highlightColors[position]}
        );
        drawable.setShape(GradientDrawable.RECTANGLE);
        drawable.setCornerRadius(35);

        // Create new ripple effect with updated colors
        ColorStateList colorStateList = new ColorStateList(
                new int[][]{
                        new int[]{android.R.attr.state_pressed},
                        new int[]{}
                },
                new int[]{
                        Color.parseColor("#20FFFFFF"),
                        Color.TRANSPARENT
                }
        );

        RippleDrawable rippleDrawable = new RippleDrawable(colorStateList, drawable, null);
        button.setBackground(rippleDrawable);

        // Set click navigation
        button.setOnClickListener(v -> {
            NavController navController = Navigation.findNavController(v);

            switch (continents[position]) {
                case "Europe":
                    navController.navigate(R.id.action_fragmentThree_to_europeFragment);
                    break;
                case "Asia":
                    navController.navigate(R.id.action_fragmentThree_to_asiaFragment);
                    break;
                case "Australia":
                    navController.navigate(R.id.action_fragmentThree_to_australiaFragment);
                    break;
                case "South America":
                    navController.navigate(R.id.action_fragmentThree_to_south_AmericaFragment);
                    break;
                case "North America":
                    navController.navigate(R.id.action_fragmentThree_to_north_AmericaFragment);
                    break;
                case "Africa":
                    navController.navigate(R.id.action_fragmentThree_to_africaFragment);
                    break;
                default:
                    Log.w("ContinentsAdapter", "onBindViewHolder: No action defined for this continent");
                    break;
            }
        });
    }

    @Override
    public int getItemCount() {
        return continents.length;
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        Button button;

        public ViewHolder(@NonNull Button itemView) {
            super(itemView);
            button = itemView;
        }
    }
}


