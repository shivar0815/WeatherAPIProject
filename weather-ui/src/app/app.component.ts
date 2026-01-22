import { Component } from '@angular/core';
import { WeatherComponent } from './features/weather/weather.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [WeatherComponent],
  template: '<app-weather></app-weather>',
  styles: []
})
export class AppComponent {
  title = 'weather-ui';
}