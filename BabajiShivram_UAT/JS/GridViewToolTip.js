
        var sTooTipId = 'divTooTip';

        // Mention here whether tooltip should be placed in Right, Left side of the cursor
        var sToolTipPlacement = "Right";

        // For Hiding tool tip
        function HideTooTip(event) {
              
            // Getting the Div element which was created dynamically on ShowToolTip function
            var divTip = document.getElementById('divTooTip');

            if (divTip) {

                while (divTip.childNodes.length > 0)
                // Removing all child content which are added inside on the Div content
                    divTip.removeChild(divTip.childNodes[0]);

            }

            // Invisibling th Div (which removed all child content)
            divTip.style.visibility = "hidden";
        }

        // For moving the tooltip with mouse is moving on the control
        function MoveToolTip(event,sToolTipPlacement) {

            // Verify if the Div content already present?
            if (document.getElementById(sTooTipId)) {

                // Get the Div content which was invisible on HideTooTip function
                var newDiv = document.getElementById(sTooTipId);

                if ('pageX' in event) {	// all browsers except IE before version 9
                    var pageX = event.pageX;
                    var pageY = event.pageY;
                }
                else {	// IE before version 9
                    var pageX = event.clientX + document.documentElement.scrollLeft;
                    var pageY = event.clientY + document.documentElement.scrollTop;
                }

                if (sToolTipPlacement == "Right")
                    newDiv.style.left = (pageX + 17) + "px";
                else // Left
                    newDiv.style.left = (pageX - (parseInt(newDiv.style.width) + 17)) + "px";

                // Portion of div when hide by browser top
                if ((pageY - (parseInt(newDiv.style.height) + 3)) < 0)
                // Showing below the cursor
                    newDiv.style.top = pageY + "px";
                else
                // Showing above the cursor
                    newDiv.style.top = (pageY - (parseInt(newDiv.style.height) + 3)) + "px";

                // Finally visibling the div
                newDiv.style.visibility = "visible";

            }
        }
        function ShowToolTip(event, sDailyProgress,sToolTipPlacement)     // Daily Progress Text
            { // For showing tool tip
            
            // Replacing back if &#96; then ' which encoded in C# code
            // sDailyProgress = sDailyProgress.replace("&#96;", "'");
            
            // Verify if the Div content already present?
            if (!document.getElementById(sTooTipId)) {

                // If not create a new Div element
                var newDiv = document.createElement("div");
                // Set the id for the Div element to refer further
                newDiv.setAttribute("id", sTooTipId);

                // Add it to the page
                document.body.appendChild(newDiv);

            }
            else
            // Get the Div content which was invisible on HideTooTip function
                var newDiv = document.getElementById(sTooTipId);

            // Here what we are assiging the tooltip innerHTML to Div (newDiv) control
            // We have to make the html script containing the database data which are comming thro parameter
            // here we can do any one of the three ways as explained above
            // Start - Below is the line you required to assign the Html script.
            // Directly constructing the html script and assigning the innerHTML to Div.
            newDiv.innerHTML = "<table>" +
                "<tr><td>" + sDailyProgress + "</td></tr>" +
                "</table>";

            // End -

            newDiv.style.zIndex = 2;
            newDiv.style.fontFamily = "Calibri";
            newDiv.style.fontSize = "9pt";
            newDiv.style.padding = "3px";
            newDiv.style.backgroundColor = "#A9D0F5";
            newDiv.style.border = "solid 1px #08088A";
            newDiv.style.width = "240px";
            newDiv.style.height = "140px";
            newDiv.style.overflow = "hidden";
            // Absolute make the div floating on the screen.
            newDiv.style.position = "absolute";

            if ('pageX' in event) {	// all browsers except IE before version 9
                var pageX = event.pageX;
                var pageY = event.pageY;
            }
            else {	// IE before version 9
                var pageX = event.clientX + document.documentElement.scrollLeft;
                var pageY = event.clientY + document.documentElement.scrollTop;
            }

            if (sToolTipPlacement == "Right")
                newDiv.style.left = (pageX + 17) + "px";
            else // Left
                newDiv.style.left = (pageX - (parseInt(newDiv.style.width) + 17)) + "px";

            // Portion of div when hide by browser top
            if ((pageY - (parseInt(newDiv.style.height) + 3)) < 0)
            // Showing below the cursor
                newDiv.style.top = pageY + "px";
            else
            // Showing above the cursor
                newDiv.style.top = (pageY - (parseInt(newDiv.style.height) + 3)) + "px";

            // Finally visibling the div which has the data
            newDiv.style.visibility = "visible";
        }

    