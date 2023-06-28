import { Pipe, PipeTransform } from "@angular/core";
import { SummaryTagDto, TodoItemDto } from "../web-api-client";


@Pipe({
    name:'summaryTagsFilter',
    pure: false
})
export class SummaryTagsFilter implements PipeTransform {
    transform(tags: SummaryTagDto[], searchText: string) {
        if (!searchText )
            return tags;

        return tags.filter(
            (v) => v.name.toLowerCase().match(searchText.toLowerCase())
        );
    }
}