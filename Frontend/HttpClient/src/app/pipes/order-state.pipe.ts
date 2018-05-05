import { Pipe, PipeTransform } from '@angular/core';
import { OrderState } from '../models/order';

@Pipe({
    name: 'orderState'
})
export class OrderStatePipe implements PipeTransform {

    transform(type: any): string {
        switch (type) {
            case OrderState.InProcess: {
                type = 'В процессе';
                break;
            }
            case OrderState.Ready: {
                type = 'Готов';
                break;
            }
            case OrderState.Undefined: {
                type = 'Не определено';
                break;
            }
            case OrderState.Error: {
                type = 'Ошибка';
                break;
            }
        }
        return type;
    }

}
