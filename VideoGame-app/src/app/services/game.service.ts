import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface VideoGame {
  id: number;
  title: string;
  genre: string;
  publisher: string;
  releaseDate: string; // "2026-09-24" format from your Swagger
  metacriticScore: number;
}

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private http = inject(HttpClient);
  // Base URL pointing to your active SSL local port from Swagger
  private apiUrl = 'https://localhost:7076/api/VideoGames'; 

  // Central state management using a writable Signal
  private gamesSignal = signal<VideoGame[]>([]);
  games = this.gamesSignal.asReadonly();

  // Fetch all games from the API backend
  loadGames(): void {
    this.http.get<VideoGame[]>(this.apiUrl).subscribe({
      next: (data) => this.gamesSignal.set(Array.isArray(data) ? data : []),
      error: (err) => {
        console.error('Failed to load games from backend API:', err);
        this.gamesSignal.set([]);
      }
    });
  }

  // Get a single game entry by ID
  getGameById(id: number) {
    return this.http.get<VideoGame>(`${this.apiUrl}/${id}`);
  }

  // Update a game entry (Sends a PUT request to the backend)
  updateGame(id: number, game: VideoGame) {
    return this.http.put(`${this.apiUrl}/${id}`, game);
  }

  createGame(game: Omit<VideoGame, 'id'>) {
  return this.http.post(this.apiUrl, game);
}
}
