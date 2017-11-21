import { Pipe, PipeTransform } from '@angular/core';
import { EquipmentType } from '../models/equipment';

@Pipe({
    name: 'equipmentType'
})
export class EquipmentTypePipe implements PipeTransform {

    transform(type: any): string {
        switch (type) {
            case 'Transport': {
                type = 'Транспорт';
                break;
            }
            case 'MachiningTool': {
                type = 'Станок';
                break;
            }
            case 'Undefined': {
                type = 'Не определено';
                break;
            }
            case 'AssemblyWorkplace': {
                type = 'Сборочное место';
                break;
            }
        }
        return type;
    }

}
