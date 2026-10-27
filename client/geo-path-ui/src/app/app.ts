import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

/** Root application shell used to host the learner journey routes. */
@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  styleUrl: './app.scss',
  template: '<router-outlet />',
})
export class App {}
