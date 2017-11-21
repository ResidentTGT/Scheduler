import { Pipe, PipeTransform } from '@angular/core';
import { OperationType } from '../models/operation';

@Pipe({
    name: 'operationType'
})
export class OperationTypePipe implements PipeTransform {

    transform(type: any): string {
        switch (type) {
            case 'Transport': {
                type = 'Транспорт';
                break;
            }
            case 'Machining': {
                type = 'Обработка';
                break;
            }
            case 'Undefined': {
                type = 'Не определено';
                break;
            }
            case 'Assembling': {
                type = 'Сборка';
                break;
            }
        }
        return type;
    }

}
