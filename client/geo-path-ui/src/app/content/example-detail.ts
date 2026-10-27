import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ContentApiService, GeometryExample } from './content-api.service';

@Component({
  selector: 'app-example-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './example-detail.html',
})
export class ExampleDetailComponent implements OnInit {
  example: GeometryExample | null = null;
  contextTitle = '';
  contextText = '';
  isLoading = true;
  isContextLoading = false;
  errorMessage = '';
  contextError = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly contentApi: ContentApiService,
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.errorMessage = 'The requested example could not be found.';
      this.isLoading = false;
      return;
    }
    void this.loadExample(id);
  }

  async showTheorem(id: string): Promise<void> {
    this.isContextLoading = true;
    this.contextError = '';
    try {
      const theorem = await this.contentApi.getTheorem(id);
      this.contextTitle = theorem.name;
      this.contextText = [theorem.statement, theorem.explanation].filter(Boolean).join('\n\n');
    } catch (error) {
      this.contextError = (error as Error).message;
    } finally {
      this.isContextLoading = false;
    }
  }

  async showDefinition(id: string): Promise<void> {
    this.isContextLoading = true;
    this.contextError = '';
    try {
      const definition = await this.contentApi.getDefinition(id);
      this.contextTitle = definition.name;
      this.contextText = definition.description;
    } catch (error) {
      this.contextError = (error as Error).message;
    } finally {
      this.isContextLoading = false;
    }
  }

  private async loadExample(id: string): Promise<void> {
    try {
      this.example = await this.contentApi.getExample(id);
    } catch (error) {
      this.errorMessage = (error as Error).message;
    } finally {
      this.isLoading = false;
    }
  }
}
