using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RDSoft.HackerNewsAggregator.Application.Interfaces
{
	public interface IFactory<out T>
	{
		T Create();
	}
}
