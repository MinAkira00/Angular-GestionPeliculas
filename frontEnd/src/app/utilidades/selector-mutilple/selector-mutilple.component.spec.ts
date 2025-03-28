import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectorMutilpleComponent } from './selector-mutilple.component';

describe('SelectorMutilpleComponent', () => {
  let component: SelectorMutilpleComponent;
  let fixture: ComponentFixture<SelectorMutilpleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectorMutilpleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectorMutilpleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
