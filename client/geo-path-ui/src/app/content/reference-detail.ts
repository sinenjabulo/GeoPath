import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ContentApiService, GeometryDefinition, Theorem } from './content-api.service';

@Component({
  selector: 'app-reference-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './reference-detail.html',
})
export class ReferenceDetailComponent implements OnInit {
  kind: 'theorem' | 'definition' = 'theorem';
  theorem: Theorem | null = null;
  definition: GeometryDefinition | null = null;
  isLoading = true;
  errorMessage = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly contentApi: ContentApiService,
  ) {}

  get name(): string {
    return this.theorem?.name ?? this.definition?.name ?? '';
  }

  get description(): string {
    if (this.theorem) {
      return [this.theorem.statement, this.theorem.explanation].filter(Boolean).join('\n\n');
    }
    return this.definition?.description ?? '';
  }

  get libraryUrl(): string {
    return this.kind === 'theorem' ? '/theorems' : '/definitions';
  }

  ngOnInit(): void {
    this.kind = this.route.snapshot.data['kind'] as 'theorem' | 'definition';
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.errorMessage = 'The requested reference could not be found.';
      this.isLoading = false;
      return;
    }
    void this.loadReference(id);
  }

  private async loadReference(id: string): Promise<void> {
    try {
      if (this.kind === 'theorem') {
        this.theorem = await this.contentApi.getTheorem(id);
      } else {
        this.definition = await this.contentApi.getDefinition(id);
      }
    } catch (error) {
      this.errorMessage = (error as Error).message;
    } finally {
      this.isLoading = false;
    }
  }
}
