import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LikeComponent } from './like.component';

describe('LikeComponent', () => {
  let component: LikeComponent;
  let fixture: ComponentFixture<LikeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LikeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LikeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
