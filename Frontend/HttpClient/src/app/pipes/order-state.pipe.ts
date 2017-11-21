import { Pipe, PipeTransform } from '@angular/core';
import { OrderState } from '../models/order';

@Pipe({
    name: 'orderState'
})
export class OrderStatePipe implements PipeTransform {

    transform(type: any): string {
        switch (type) {
            case 'InProcess': {
                type = 'В процессе';
                break;
            }
            case 'Ready': {
                type = 'Готов';
                break;
            }
            case 'Undefined': {
                type = 'Не определено';
                break;
            }

        }
        return type;
    }

}
