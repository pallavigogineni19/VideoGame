import { Routes } from '@angular/router';
import { GameCatalogue} from './components/game-catalogue/game-catalogue';
import { GameEdit } from './components/game-edit/game-edit';

export const routes: Routes = [
  // 1. Default route redirects straight to the catalogue page
  { path: '', redirectTo: 'catalogue', pathMatch: 'full' },
  
  // 2. Browsing page
  { path: 'catalogue', component: GameCatalogue },
  
  // 3. Editing page (expects a dynamic :id parameter for the game)
  { path: 'edit/:id', component: GameEdit },

   { path: 'create', component: GameEdit }
];
