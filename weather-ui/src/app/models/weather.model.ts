export interface WeatherData {
  date: string;
  minTemp: number | null;
  maxTemp: number | null;
  precipitation: number | null;
  status: string;
  errorMessage: string | null;
}

export type SortField = 'date' | 'minTemp' | 'maxTemp' | 'precipitation';
export type SortDirection = 'asc' | 'desc';