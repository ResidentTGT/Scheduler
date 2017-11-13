import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { BackendApiService } from '../../services/backend-api.service';
import { MatFormFieldControl } from '@angular/material';
import { Equipment, EquipmentType } from '../../models/equipment';

@Component({
    selector: 'sch-equipments',
    templateUrl: './equipments.component.html',
    styleUrls: ['./equipments.component.scss']
})
export class EquipmentsComponent implements OnInit {

    equipments: Equipment[] = [];

    name: '';
    description: '';
    type: EquipmentType = EquipmentType.Undefined;

    constructor(private _api: BackendApiService) {
    }

    ngOnInit() {
        this.getEquipments();
    }

    private getEquipments() {
        this._api.getEquipments()
            .do(equipments => this.equipments = equipments)
            .catch(resp => {
                alert(`Не удалось загрузить список оборудования по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe();
    }

    public createEquipment() {
        const equipment: Equipment = {
            name: this.name,
            description: this.description,
            type: this.type
        };

        this._api.createEquipment(equipment)
            .catch(resp => {
                alert(`Не удалось добавить оборудование по причине: ${JSON.stringify(resp.json())}`);
                return Observable.empty();
            })
            .subscribe(id => {
                equipment.id = id;
                this.equipments.push(equipment);
            });
    }

}




