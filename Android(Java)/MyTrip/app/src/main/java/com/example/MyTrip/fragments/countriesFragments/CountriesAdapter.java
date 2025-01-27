package com.example.MyTrip.fragments.countriesFragments;

import android.graphics.Color;
import android.graphics.drawable.GradientDrawable;
import android.util.Log;
import android.util.TypedValue;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.Filter;
import android.widget.Filterable;
import android.widget.TextView;

import com.example.MyTrip.R;

import androidx.annotation.NonNull;
import androidx.cardview.widget.CardView;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class CountriesAdapter extends RecyclerView.Adapter<CountriesAdapter.ViewHolder> implements Filterable {

    private final List<String> countries;
    private List<String> filteredCountries;
    private static final List<CountryAction> countryActions = new ArrayList<>();

    static {
        countryActions.add(new CountryAction("France", R.id.action_europeFragment_to_franceFragment));
        countryActions.add(new CountryAction("Czech Republic", R.id.action_europeFragment_to_czechRepublicFragment));
        countryActions.add(new CountryAction("Hungary", R.id.action_europeFragment_to_hungaryFragment));
        //THE PLACE TO ADD MORE COUNTRIES
    }

    public CountriesAdapter(List<String> countries) {
        this.countries = countries;
        this.filteredCountries = new ArrayList<>(countries);
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.country_item, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        String country = filteredCountries.get(position);
        holder.countryName.setText(country);

        // Set random background gradient
        int[] colors = getRandomGradientColors();
        GradientDrawable gradient = new GradientDrawable(
                GradientDrawable.Orientation.TL_BR,
                colors
        );
        gradient.setCornerRadius(30);
        holder.cardView.setBackground(gradient);

        // Add ripple effect
        TypedValue outValue = new TypedValue();
        holder.itemView.getContext().getTheme().resolveAttribute(
                android.R.attr.selectableItemBackground, outValue, true);
        holder.cardView.setForeground(holder.itemView.getContext()
                .getDrawable(outValue.resourceId));

        holder.cardView.setOnClickListener(v -> {
            // Add click animation
            holder.cardView.animate()
                    .scaleX(0.95f)
                    .scaleY(0.95f)
                    .setDuration(100)
                    .withEndAction(() -> {
                        holder.cardView.animate()
                                .scaleX(1f)
                                .scaleY(1f)
                                .setDuration(100)
                                .start();

                        // Navigate after animation
                        CountryAction action = countryActions.stream()
                                .filter(ca -> ca.getCountryName().equals(country))
                                .findFirst()
                                .orElse(null);

                        if (action != null) {
                            NavController navController = Navigation.findNavController(v);
                            navController.navigate(action.getActionId());
                        }
                    })
                    .start();
        });
    }

    private int[] getRandomGradientColors() {
        // Array of material design colors
        int[][] colorPairs = {
                {0xFF2196F3, 0xFF03A9F4},  // Bright Blue
                {0xFFE91E63, 0xFFFF4081},  // Pink
                {0xFF9C27B0, 0xFFBA68C8},  // Purple
                {0xFF00BCD4, 0xFF4DD0E1},  // Cyan
                {0xFF4CAF50, 0xFF81C784},  // Green
                {0xFFFF9800, 0xFFFFB74D},  // Orange
                {0xFF673AB7, 0xFF9575CD},  // Deep Purple
                {0xFF009688, 0xFF4DB6AC},  // Teal
                {0xFFFF5722, 0xFFFF8A65}   // Deep Orange
        };

        int randomIndex = new Random().nextInt(colorPairs.length);
        return colorPairs[randomIndex];
    }

    @Override
    public int getItemCount() {
        return filteredCountries.size();
    }

    @Override
    public Filter getFilter() {
        return new Filter() {
            @Override
            protected FilterResults performFiltering(CharSequence constraint) {
                List<String> filteredList = new ArrayList<>();
                if (constraint == null || constraint.length() == 0) {
                    filteredList.addAll(countries);
                } else {
                    String filterPattern = constraint.toString().toLowerCase().trim();
                    for (String country : countries) {
                        if (country.toLowerCase().contains(filterPattern)) {
                            filteredList.add(country);
                        }
                    }
                }

                FilterResults results = new FilterResults();
                results.values = filteredList;
                return results;
            }

            @Override
            protected void publishResults(CharSequence constraint, FilterResults results) {
                filteredCountries.clear();
                filteredCountries.addAll((List<String>) results.values);
                notifyDataSetChanged();
            }
        };
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        CardView cardView;
        TextView countryName;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            cardView = itemView.findViewById(R.id.country_card);
            countryName = itemView.findViewById(R.id.country_name);
        }
    }
}
