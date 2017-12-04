import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { Order } from '../../../models/order';

@Component({
    selector: 'sch-view-production-item-quantums-groups',
    templateUrl: './view-production-item-quantums-groups.component.html',
    styleUrls: ['./view-production-item-quantums-groups.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewProductionItemQuantumsGroupsComponent implements OnInit {

    @Input()
    public order: Order;

    @Input()
    public selectedBlock: Map<number, number>;

    constructor() { }

    ngOnInit() {
debugger;
    }

}
