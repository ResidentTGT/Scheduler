import { Component, OnInit } from '@angular/core';
import { Order } from '../../../models/order';
import { BackendApiService } from '../../../services/backend-api.service';
import { Observable } from 'rxjs/Rx';

@Component({
  selector: 'sch-view-order-graph',
  templateUrl: './view-order-graph.component.html',
  styleUrls: ['./view-order-graph.component.scss']
})
export class ViewOrderGraphComponent implements OnInit {

    public loading: boolean;

    public selectedBlock: { orderQuantumIndex: number, offset: number };

    public selectedGroup: { orderQuantumIndex: number, groupIndex: number, offset: number };

    public calculatedOrder: Order = null;

    constructor(private _api: BackendApiService) { }

    ngOnInit() {
    }

}
