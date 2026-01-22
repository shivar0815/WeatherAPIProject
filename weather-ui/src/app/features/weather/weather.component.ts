import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { WeatherService } from '../../core/services/weather.service';
import { WeatherData, SortField, SortDirection } from '../../models/weather.model';

@Component({
  selector: 'app-weather',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  providers: [WeatherService],
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  weatherData: WeatherData[] = [];
  filteredData: WeatherData[] = [];
  loading = false;
  error: string | null = null;
  selectedWeather: WeatherData | null = null;

  sortField: SortField = 'date';
  sortDirection: SortDirection = 'asc';
  minTempFilter: number | null = null;

  constructor(private weatherService: WeatherService) { }

  ngOnInit(): void {
    this.loadWeatherData();
  }

  loadWeatherData(): void {
    this.loading = true;
    this.error = null;

    this.weatherService.getWeatherData().subscribe({
      next: (data) => {
        this.weatherData = data;
        this.filteredData = [...data];
        this.applySorting();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Failed to load weather data';
        this.loading = false;
      }
    });
  }

  sortBy(field: SortField): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'asc';
    }
    this.applySorting();
  }

  applySorting(): void {
    this.filteredData.sort((a, b) => {
      let aValue: any;
      let bValue: any;

      switch (this.sortField) {
        case 'date':
          aValue = new Date(a.date).getTime();
          bValue = new Date(b.date).getTime();
          break;
        case 'minTemp':
          aValue = a.minTemp ?? -Infinity;
          bValue = b.minTemp ?? -Infinity;
          break;
        case 'maxTemp':
          aValue = a.maxTemp ?? -Infinity;
          bValue = b.maxTemp ?? -Infinity;
          break;
        case 'precipitation':
          aValue = a.precipitation ?? -Infinity;
          bValue = b.precipitation ?? -Infinity;
          break;
      }

      if (this.sortDirection === 'asc') {
        return aValue > bValue ? 1 : -1;
      } else {
        return aValue < bValue ? 1 : -1;
      }
    });
  }

  filterByMinTemp(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    
    if (value === '') {
      this.minTempFilter = null;
      this.filteredData = [...this.weatherData];
    } else {
      this.minTempFilter = parseFloat(value);
      this.filteredData = this.weatherData.filter(w => 
        w.minTemp !== null && w.minTemp >= this.minTempFilter!
      );
    }
    this.applySorting();
  }

  selectWeather(weather: WeatherData): void {
    this.selectedWeather = weather;
  }

  closeDetails(): void {
    this.selectedWeather = null;
  }

  getSortIcon(field: SortField): string {
    if (this.sortField !== field) return '';
    return this.sortDirection === 'asc' ? '↑' : '↓';
  }

  retry(): void {
    this.loadWeatherData();
  }
}