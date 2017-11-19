import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateProductionItemComponent } from './create-production-item.component';

describe('CreateProductionItemComponent', () => {
  let component: CreateProductionItemComponent;
  let fixture: ComponentFixture<CreateProductionItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateProductionItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateProductionItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
