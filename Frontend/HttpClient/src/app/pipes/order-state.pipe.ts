import { Pipe, PipeTransform } from '@angular/core';
import { OrderState } from '../models/order';

@Pipe({
    name: 'orderState'
})
export class OrderStatePipe implements PipeTransform {

    transform(type: any): string {
        switch (type) {
            case 'InProcess' || OrderState.InProcess: {
                type = 'В процессе';
                break;
            }
            case 'Ready' || OrderState.Ready: {
                type = 'Готов';
                break;
            }
            case 'Undefined' || OrderState.Undefined: {
                type = 'Не определено';
                break;
            }

        }
        return type;
    }

}
