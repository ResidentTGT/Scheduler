import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalculateOrdersComponent } from './calculate-orders.component';

describe('CalculateOrdersComponent', () => {
  let component: CalculateOrdersComponent;
  let fixture: ComponentFixture<CalculateOrdersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalculateOrdersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalculateOrdersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
