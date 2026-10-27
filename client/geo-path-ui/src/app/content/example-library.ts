import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ContentApiService, GeometryExample } from './content-api.service';

@Component({
  selector: 'app-example-library',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './example-library.html',
})
export class ExampleLibraryComponent implements OnInit {
  examples: GeometryExample[] = [];
  searchTerm = '';
  selectedTopic = '';
  selectedDifficulty = '';
  isLoading = true;
  errorMessage = '';

  constructor(private readonly contentApi: ContentApiService) {}

  ngOnInit(): void {
    void this.loadExamples();
  }

  get topics(): string[] {
    return [...new Set(this.examples.map((example) => example.topicName))].sort();
  }

  get difficulties(): string[] {
    return [...new Set(this.examples.map((example) => example.difficulty))].sort();
  }

  get filteredExamples(): GeometryExample[] {
    const query = this.searchTerm.trim().toLocaleLowerCase();
    return this.examples.filter((example) => {
      const matchesQuery =
        !query ||
        example.title.toLocaleLowerCase().includes(query) ||
        example.prompt.toLocaleLowerCase().includes(query) ||
        example.topicName.toLocaleLowerCase().includes(query);
      return (
        matchesQuery &&
        (!this.selectedTopic || example.topicName === this.selectedTopic) &&
        (!this.selectedDifficulty || example.difficulty === this.selectedDifficulty)
      );
    });
  }

  async loadExamples(): Promise<void> {
    this.isLoading = true;
    this.errorMessage = '';
    try {
      this.examples = await this.contentApi.getExamples();
    } catch (error) {
      this.errorMessage = (error as Error).message;
    } finally {
      this.isLoading = false;
    }
  }
}
