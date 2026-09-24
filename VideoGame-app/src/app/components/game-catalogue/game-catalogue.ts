import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { GameService } from '../../services/game.service';

@Component({
  selector: 'app-game-catalogue',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './game-catalogue.html'
})
export class GameCatalogue implements OnInit {
  private gameService = inject(GameService);

  // Expose the read-only data signal directly to your HTML template
  games = this.gameService.games;

  ngOnInit(): void {
    this.gameService.loadGames();
  }
}
