import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionItemsComponent } from './production-items.component';

describe('ProductionItemsComponent', () => {
  let component: ProductionItemsComponent;
  let fixture: ComponentFixture<ProductionItemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductionItemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
