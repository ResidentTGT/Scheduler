import { Pipe, PipeTransform } from '@angular/core';
import { OperationType } from '../models/operation';

@Pipe({
  name: 'operationType'
})
export class OperationTypePipe implements PipeTransform {

    transform(type: any): string {
        switch (type) {
            case 'Transport' || OperationType.Transport: {
                type = 'Транспорт';
                break;
            }
            case 'Machining' || OperationType.Machining: {
                type = 'Обработка';
                break;
            }
            case 'Undefined' || OperationType.Undefined: {
                type = 'Не определено';
                break;
            }
            case 'Assembling' || OperationType.Assembling: {
                type = 'Сборка';
                break;
            }
        }
        return type;
    }

}
