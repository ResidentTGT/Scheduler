import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProductionItemQuantumGroupComponent } from './view-production-item-quantum-group.component';

describe('ViewProductionItemQuantumGroupComponent', () => {
  let component: ViewProductionItemQuantumGroupComponent;
  let fixture: ComponentFixture<ViewProductionItemQuantumGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewProductionItemQuantumGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewProductionItemQuantumGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
