import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ContentApiService, GeometryDefinition, Theorem } from './content-api.service';

@Component({
  selector: 'app-reference-library',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './reference-library.html',
})
export class ReferenceLibraryComponent implements OnInit {
  kind: 'theorem' | 'definition' = 'theorem';
  theorems: Theorem[] = [];
  definitions: GeometryDefinition[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly contentApi: ContentApiService,
  ) {}

  get title(): string {
    return this.kind === 'theorem' ? 'Theorem library' : 'Definition library';
  }

  get description(): string {
    return this.kind === 'theorem'
      ? 'Review published geometry theorems and their statements.'
      : 'Look up key geometry terms and their meanings.';
  }

  get entries(): Array<Theorem | GeometryDefinition> {
    return this.kind === 'theorem' ? this.theorems : this.definitions;
  }

  ngOnInit(): void {
    this.kind = this.route.snapshot.data['kind'] as 'theorem' | 'definition';
    void this.loadEntries();
  }

  entryText(entry: Theorem | GeometryDefinition): string {
    return 'statement' in entry
      ? [entry.statement, entry.explanation].filter(Boolean).join(' ')
      : entry.description;
  }

  detailUrl(id: string): string[] {
    return [`/${this.kind === 'theorem' ? 'theorems' : 'definitions'}`, id];
  }

  async loadEntries(): Promise<void> {
    this.isLoading = true;
    this.errorMessage = '';
    try {
      if (this.kind === 'theorem') {
        this.theorems = await this.contentApi.getTheorems();
      } else {
        this.definitions = await this.contentApi.getDefinitions();
      }
    } catch (error) {
      this.errorMessage = (error as Error).message;
    } finally {
      this.isLoading = false;
    }
  }
}
