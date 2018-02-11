import { Injectable } from '@angular/core';

@Injectable()
export class HelperService {

  constructor() { }

  public isNullOrWhitespace(input: string) {
    return !input || !input.trim();
  }

}
