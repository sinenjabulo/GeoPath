import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

export interface GeometryExample {
  id: string;
  title: string;
  prompt: string;
  guidedFlowJson: string | null;
  difficulty: string;
  topicId: string;
  topicName: string;
  theorems: TheoremReference[];
  definitions: DefinitionReference[];
}

export interface TheoremReference {
  id: string;
  name: string;
  statement: string;
}

export interface DefinitionReference {
  id: string;
  name: string;
  description: string;
}

export interface Theorem {
  id: string;
  name: string;
  statement: string;
  explanation: string | null;
}

export interface GeometryDefinition {
  id: string;
  name: string;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class ContentApiService {
  private readonly baseUrl = 'http://localhost:5229/api';

  constructor(private readonly http: HttpClient) {}

  getExamples(): Promise<GeometryExample[]> {
    return this.get<GeometryExample[]>('/examples');
  }

  getExample(id: string): Promise<GeometryExample> {
    return this.get<GeometryExample>(`/examples/${encodeURIComponent(id)}`);
  }

  getTheorems(): Promise<Theorem[]> {
    return this.get<Theorem[]>('/theorems');
  }

  getTheorem(id: string): Promise<Theorem> {
    return this.get<Theorem>(`/theorems/${encodeURIComponent(id)}`);
  }

  getDefinitions(): Promise<GeometryDefinition[]> {
    return this.get<GeometryDefinition[]>('/definitions');
  }

  getDefinition(id: string): Promise<GeometryDefinition> {
    return this.get<GeometryDefinition>(`/definitions/${encodeURIComponent(id)}`);
  }

  private async get<T>(path: string): Promise<T> {
    try {
      return await firstValueFrom(this.http.get<T>(`${this.baseUrl}${path}`));
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        if (error.status === 0) {
          throw new Error('GeoPath could not reach the content service. Check that the API is running.');
        }

        if (error.status === 404) {
          throw new Error('This published content is no longer available.');
        }
      }

      throw new Error('Unable to load this content. Please try again.');
    }
  }
}
