import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';

/** Starts the Angular application and reports any bootstrap failure. */
bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
