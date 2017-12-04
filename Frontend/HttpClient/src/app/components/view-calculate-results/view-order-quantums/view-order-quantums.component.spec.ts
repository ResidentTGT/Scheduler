import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOrderQuantumsComponent } from './view-order-quantums.component';

describe('ViewOrderQuantumsComponent', () => {
  let component: ViewOrderQuantumsComponent;
  let fixture: ComponentFixture<ViewOrderQuantumsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOrderQuantumsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOrderQuantumsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
