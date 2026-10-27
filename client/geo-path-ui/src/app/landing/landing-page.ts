import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.scss',
})
export class LandingPageComponent {
  readonly features = [
    {
      title: 'Guided reasoning',
      description: 'Students move through abstraction, decomposition, pattern recognition, and solving.',
    },
    {
      title: 'Proof support',
      description: 'Each example gives learners the context they need to build a clear statement-and-reason proof.',
    },
    {
      title: 'Progress that sticks',
      description: 'Saved attempts let a learner resume in-progress work and revisit what they completed.',
    },
  ];

  readonly sampleSteps = [
    'Review the diagram and identify the givens',
    'Break the task into the relevant parts',
    'Connect the key geometry facts and patterns',
    'Build a proof using the correct statements and reasons',
  ];
}
