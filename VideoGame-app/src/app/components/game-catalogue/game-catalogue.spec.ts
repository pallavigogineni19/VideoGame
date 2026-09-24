import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GameCatalogue } from './game-catalogue';

describe('GameCatalogue', () => {
  let component: GameCatalogue;
  let fixture: ComponentFixture<GameCatalogue>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameCatalogue],
    }).compileComponents();

    fixture = TestBed.createComponent(GameCatalogue);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
