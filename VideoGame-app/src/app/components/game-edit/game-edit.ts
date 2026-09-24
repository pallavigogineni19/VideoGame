import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GameService, VideoGame } from '../../services/game.service';

@Component({
  selector: 'app-game-edit',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './game-edit.html'
})
export class GameEdit implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private gameService = inject(GameService);
  private fb = inject(FormBuilder);

  editForm!: FormGroup;
  currentGameId: number | null = null; // Changed to allow null for creation mode
  isEditMode = signal(false);          // State tracking toggle for UI dynamic text

  isSaving = signal(false);
  errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.editForm = this.fb.group({
      id: [0],
      title: ['', [Validators.required, Validators.maxLength(100)]],
      genre: ['', [Validators.required]],
      publisher: ['', [Validators.required]],
      releaseDate: ['', [Validators.required]],
      metacriticScore: [0, [Validators.min(0), Validators.max(100)]]
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.currentGameId = +idParam;
      this.isEditMode.set(true); // Switch context flags to editing mode
      this.fetchGameData(this.currentGameId);
    }
  }

  private fetchGameData(id: number): void {
    this.gameService.getGameById(id).subscribe({
      next: (game: VideoGame) => {
        const formattedDate = game.releaseDate ? game.releaseDate.split('T')[0] : '';

        this.editForm.patchValue({
          ...game,
          releaseDate: formattedDate
        });
      },
      error: (err) => {
        this.errorMessage.set('Could not load game metadata from backend.');
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    if (this.editForm.invalid) {
      return;
    }

    this.isSaving.set(true);
    const updatedModel: VideoGame = this.editForm.value;

    // Conditionally fork execution branches based on active routing criteria
    if (this.isEditMode()) {
      // Execute PUT request for editing updates
      this.gameService.updateGame(this.currentGameId!, updatedModel).subscribe({
        next: () => this.handleSuccess(),
        error: (err) => this.handleError(err)
      });
    } else {
      // Execute POST request for database generation entries (omitting the ID field tracking)
      const { id, ...newGamePayload } = updatedModel;
      this.gameService.createGame(newGamePayload).subscribe({
        next: () => this.handleSuccess(),
        error: (err) => this.handleError(err)
      });
    }
  }

  private handleSuccess(): void {
    this.isSaving.set(false);
    this.router.navigate(['/catalogue']);
  }

  private handleError(err: any): void {
    this.isSaving.set(false);
    this.errorMessage.set('Backend operation failed. Please verify your fields.');
    console.error(err);
  }
}
