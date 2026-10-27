import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ContentApiService, GeometryExample } from './content-api.service';
import { ExampleLibraryComponent } from './example-library';

describe('ExampleLibraryComponent', () => {
  const examples: GeometryExample[] = [
    {
      id: 'one',
      title: 'Angles in a circle',
      prompt: 'Find the angle',
      guidedFlowJson: null,
      difficulty: 'Medium',
      topicId: 'circle',
      topicName: 'Circle geometry',
      theorems: [],
      definitions: [],
    },
    {
      id: 'two',
      title: 'Triangle angles',
      prompt: 'Find the missing angle',
      guidedFlowJson: null,
      difficulty: 'Easy',
      topicId: 'triangle',
      topicName: 'Triangle geometry',
      theorems: [],
      definitions: [],
    },
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExampleLibraryComponent],
      providers: [
        provideRouter([]),
        {
          provide: ContentApiService,
          useValue: { getExamples: async () => examples },
        },
      ],
    }).compileComponents();
  });

  it('filters published examples by search, topic, and difficulty', async () => {
    const fixture = TestBed.createComponent(ExampleLibraryComponent);
    fixture.detectChanges();
    await fixture.whenStable();

    fixture.componentInstance.searchTerm = 'circle';
    fixture.componentInstance.selectedTopic = 'Circle geometry';
    fixture.componentInstance.selectedDifficulty = 'Medium';

    expect(fixture.componentInstance.filteredExamples.map((example) => example.id)).toEqual(['one']);
  });
});
