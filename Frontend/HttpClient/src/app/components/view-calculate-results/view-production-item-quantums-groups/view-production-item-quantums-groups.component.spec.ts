import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProductionItemQuantumsGroupsComponent } from './view-production-item-quantums-groups.component';

describe('ViewProductionItemQuantumsGroupsComponent', () => {
    let component: ViewProductionItemQuantumsGroupsComponent;
    let fixture: ComponentFixture<ViewProductionItemQuantumsGroupsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ViewProductionItemQuantumsGroupsComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ViewProductionItemQuantumsGroupsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
