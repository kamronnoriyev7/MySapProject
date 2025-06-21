export interface IncomingPaymentDto {
  docEntry: number;
  docDate: Date;
  cardName?: string;
  cardCode?: string;
  cashSum: number;
  docCurrency?: string;
  remarks?: string;
}