import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ThumbnailExtractorComponent } from './thumbnail-extractor.component';

describe('ThumbnailExtractorComponent', () => {
  let component: ThumbnailExtractorComponent;
  let fixture: ComponentFixture<ThumbnailExtractorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ThumbnailExtractorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ThumbnailExtractorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
